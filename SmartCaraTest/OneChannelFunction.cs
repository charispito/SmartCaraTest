using SmartCaraTest.data;
using SmartCaraTest.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCaraTest
{
    partial class OneChannelWindow
    {        
        public string GetDateTime()
        {
            DateTime NowDate = DateTime.Now;
            return NowDate.ToString("yyyy-MM-dd HH:mm:ss") + ":" + NowDate.Millisecond.ToString("000");
        }
       
        private string CommandToString(byte[] command)
        {
            string hex = "";
            foreach (byte b in command)
            {
                hex += " " + b.ToString("X2");
            }
            return hex;
        }

        public void Log(String msg)
        {
            string FilePath = Directory.GetCurrentDirectory() + @"\Logs\" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            string DirPath = Directory.GetCurrentDirectory() + @"\Logs";
            string temp;
            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);
            try
            {
                if (di.Exists != true) Directory.CreateDirectory(DirPath);
                if (fi.Exists != true)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        temp = string.Format("[{0}] {1}", GetDateTime(), msg);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        temp = string.Format("[{0}] {1}", GetDateTime(), msg);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        private void CheckCommand(byte[] array)
        {
            array.PrintHex(1);
            byte check = Protocol.GetCheckSum(array, 1, array.Length - 3);
            try
            {
                switch (array[2])
                {
                    case 0xA0:
                        Console.WriteLine("CheckSum: {0}, NewCheckSum: {1}, ReceivedCheckSum: {2}", check.ToString("X2"), (check ^ 0xFF).ToString("X2"), array[55].ToString("X2"));
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            setView(array);
                        }));
                        break;
                    case 0xAA:
                        Console.WriteLine("CheckSum: {0}, NewCheckSum: {1}, ReceivedCheckSum: {2}", check.ToString("X2"), (check ^ 0xFF).ToString("X2"), array[55].ToString("X2"));
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            setView(array);
                        }));
                        break;
                    case 0x99:
                        Console.WriteLine("CheckSum: {0}, NewCheckSum: {1}, ReceivedCheckSum: {2}", check.ToString("X2"), (check ^ 0xFF).ToString("X2"), array[68].ToString("X2"));
                        if (channel.ParameterMode && channel.parameterWindow != null)
                        {
                            channel.parameterWindow.setParameter(array);
                        }
                        break;
                    case 0xB9:
                        Console.WriteLine("CheckSum: {0}, NewCheckSum: {1}, ReceivedCheckSum: {2}", check.ToString("X2"), (check ^ 0xFF).ToString("X2"), array[68].ToString("X2"));
                        if (channel.ParameterMode && channel.parameterWindow != null)
                        {
                            channel.parameterWindow.setError(array);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }

        private void setView(byte[] data)
        {
            if(data.Length < 4 || data.Length != data[3])
            {
                return;
            }
            long total_second = (long)TestTime.TotalSeconds;
            double total_minute = total_second/60;
            double remain_second = (double)total_second%60/100.0;
            total_minute += remain_second;
            if(total_minute >= Chart._XMax)
            {
                Chart._XMax += 10;
                Chart.MinutesAxis.Maximum = Chart._XMax;
                Chart.setViewPort(60);
            }
            Console.WriteLine("total: {0}", total_minute);
            int motorRun = data[5];
            int heateroff = data[7]; //히터 오프타임
            int heatertemp = data[6]; //히터 온도
            int airtemp = data[8]; //배기 온도
            int airaverage = data[9];
            int airheatertemp = data[10]; //열풍 온도
            int heaterduty = data[11];
            int mode = data[15];
            int minute = data[16];
            int second = data[17];
            if(mode == 7)
            {
                channel.run = false;
            }
            else
            {
                if (!channel.run)
                {
                    channel.run = true;
                }
            }
            int t_hour = data[18];
            int t_min = data[19];
            int t_sec = data[20];
            int runTime = data[21];
            string time = string.Format("{0:D2}:{1:D2}", minute, second);
            string t_time = string.Format("{0:D2}:{1:D2}:{2:D2}", t_hour, t_min, t_sec);
            byte[] current = { data[13], data[14] };
            Array.Reverse(current);
            int currentInt = BitConverter.ToInt16(current, 0) * 10;
            float currnetDouble = (float)currentInt / 1000.0f;
            float currentfloat = (data[13] & 0xFF) << 8;
            currentfloat += (data[14] & 0xFF);
            currentfloat = currentfloat / 100.0f;
            if (currentfloat > 1)
                currentfloat = -1;
            int year = data[51];
            int month = data[52];
            int day = data[53];
            int version = data[54];
            string micom = $"{year}.{month}.{day}. ver {version}";
            byte error0 = data[49];
            byte error1 = data[50];
            int[] binary0 = Enumerable.Range(1, 8).Select(i => error0 / (1 << (8 - i)) % 2).ToArray();
            int[] binary1 = Enumerable.Range(1, 8).Select(i => error1 / (1 << (8 - i)) % 2).ToArray();
            //Array.Reverse(binary0);
            //Array.Reverse(binary1);
            string errorStr = GetErrorName(binary0, binary1);
            channel.StateContent.Content = errorStr;
            //5 = 0101 RUN CCW
            //9 = 1001 RUN STOP
            //3 = 0011 RUN CW
            //8 = 1000 STOP 
            //2 = 0010 CW
            //4 = 0100 CCW
            bool[] errors0 = new bool[8];
            bool[] errors1 = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                if (binary0[i] == 0)
                {
                    errors0[i] = true;
                }
                else
                {
                    errors0[i] = false;
                }
                if (binary1[i] == 0)
                {
                    errors1[i] = true;
                }
                else
                {
                    errors1[i] = false;
                }
            }
            if (errors1[6])
            {
                channel.Item16.cont.Content = "정상";
            }
            else
            {
                channel.Item16.cont.Content = "감지";
            }
            int motorRunTime = data[12];
            int operationTime = 10;
            int cw_on = data[22];
            int cw_off = data[23];
            int ccw_on = data[24];
            int ccw_off = data[25];
            int foreign_time = data[31];
            int foreign_current = data[32];
            int foreign_count = data[33];
            int motor_over_current = data[34];
            int motor_over_count = data[35];
            int ventile_temp = data[36];
            int heater_setting_temp = data[26];
            int heater_setting_offtime = data[27];
            int fan_duty = data[29];
            int hot_air_fan_duty = data[30];
            float heateroffTime = (float)(heateroff / 10.0f);
            DateTime now = DateTime.Now;
            if (channel.run)
            {
                ReadData read = new ReadData()
                {
                    date = DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss"),
                    mode = mode + 1,
                    remain_time = time,
                    heater_temp = heatertemp,
                    heater_off_time = heateroff,
                    air_temp = airtemp,
                    fan_speed = fan_duty,
                    hot_air_temp = airheatertemp,
                    hot_air_ontime = heaterduty,
                    motor = getMotorState(motorRun),
                    motor_current = currnetDouble
                };
                channel.WriteFile(read);
            }
            
            DetailView.operation.Value.Content = runTime;

            DetailView.motor.Value11.Content = cw_on; //motor cw on
            DetailView.motor.Value12.Content = cw_off; //motor cw off
            DetailView.motor.Value21.Content = ccw_on; //motor ccw on
            DetailView.motor.Value22.Content = ccw_off; //motor ccw off

            DetailView.material.Value.Content = foreign_time; //foreign time
            DetailView.overflow.Value11.Content = foreign_current; //overflow current1
            DetailView.overflow.Value12.Content = foreign_count; //overflow count1
            DetailView.overflow.Value21.Content = motor_over_current; //overflow current2
            DetailView.overflow.Value22.Content = motor_over_count; //overflow count2

            DetailView.temper.Value1.Content = heater_setting_temp;
            DetailView.temper.Value2.Content = heater_setting_offtime;
            DetailView.temper.Value3.Content = ventile_temp;
            DetailView.temper.Value4.Content = fan_duty;

            DetailView.heater.Value1.Content = 0;
            DetailView.heater.Value2.Content = hot_air_fan_duty;

            
            channel.Item21.cont.Content = getModelName(data[48]);
            channel.Item22.cont.Content = micom;
            channel.Item24.cont.Content = time;
            channel.Item1.cont.Content = heatertemp + "ºC";
            channel.Item2.cont.Content = airtemp + "ºC";
            channel.Item3.cont.Content = airheatertemp + "ºC";
            channel.Item5.cont.Content = fan_duty + "%";
            channel.Item15.cont.Content = hot_air_fan_duty + "%";
            channel.Item11.cont.Content = heateroffTime + "ms";
            channel.Item12.cont.Content = airaverage + "ºC";
            channel.Item13.cont.Content = heaterduty;
            channel.Item14.cont.Content = string.Format("{0:0.00A}", currentfloat / 2.0);
            channel.Item6.cont.Content = t_time;
            channel.Item4.Title = getMotorState(motorRun);
            channel.Item4.cont.Content = motorRunTime.ToString() + "s";
            channel.Item26.cont.Content = (mode + 1).ToString();
            //if (channel.run)
            //132974952879387151
            //132974952889504627
            //132974952899671426
            
            Console.WriteLine("NOW: " + DateTime.Now.ToFileTime());
                if (channel.Item1Check.IsChecked.Value)
                {
                    channel.list1.Add(new KeyValuePair<double, int>(total_minute, heatertemp));
                }
                if (channel.Item2Check.IsChecked.Value)
                {
                    channel.list2.Add(new KeyValuePair<double, int>(total_minute, airtemp));
                }
                if (channel.Item3Check.IsChecked.Value)
                {
                    channel.list3.Add(new KeyValuePair<double, int>(total_minute, airheatertemp));
                }
                if (channel.Item4Check.IsChecked.Value)
                {
                    channel.list4.Add(new KeyValuePair<double, int>(total_minute, getMotorValue(motorRun)));
                }
                if (channel.Item5Check.IsChecked.Value)
                {
                    channel.list5.Add(new KeyValuePair<double, int>(total_minute, heateroff));
                }
                if (channel.Item6Check.IsChecked.Value)
                {
                    channel.list6.Add(new KeyValuePair<double, int>(total_minute, airaverage));
                }
                if (channel.Item7Check.IsChecked.Value)
                {
                    channel.list7.Add(new KeyValuePair<double, int>(total_minute, heaterduty));
                }
                if (channel.Item8Check.IsChecked.Value)
                {
                    channel.list8.Add(new KeyValuePair<double, double>(total_minute, currnetDouble/2.0));
                }
            //}
        }

        public void OnStart()
        {
            TestTime = TimeSpan.Zero;
        }

        private string GetErrorName(int[] errors0, int[] errors1)
        {
            Array.Reverse(errors0);
            Array.Reverse(errors1);
            StringBuilder builder = new StringBuilder();
            int cnt = 0;
            if (errors0 != null && errors0.Length > 0)
                for (int i = 0; i < errors0.Length; i++)
                {
                    if (errors0[i] == 1)
                    {
                        cnt++;
                        switch (i)
                        {
                            case 0:
                                builder.AppendLine("모터 과부하", true);
                                break;
                            case 1:
                                builder.AppendLine("모터 단선", true);
                                break;
                            case 2:
                                builder.AppendLine("히터 동작 이상", true);
                                break;
                            case 3:
                                if (errors0[2] != 1)
                                    builder.AppendLine("히터 동작 이상", true);
                                else
                                    cnt--;
                                break;
                            case 4:
                                builder.AppendLine("히터 센서 이상", true);
                                break;
                            case 5:
                                builder.AppendLine("배기 온도 이상", true);
                                break;
                            case 6:
                                builder.AppendLine("배기 센서 이상", true);
                                break;
                            case 7:
                                builder.AppendLine("배기 팬 이상", true);
                                break;
                        }
                    }
                }
            if (errors1 != null && errors1.Length > 0)
                for (int i = 0; i < errors1.Length; i++)
                {
                    if (errors1[i] == 1)
                    {
                        cnt++;
                        switch (i)
                        {
                            case 0:
                                builder.AppendLine("이물질감지", true);
                                break;
                            case 1:
                                builder.AppendLine("도어 열림", true);
                                break;
                            case 2:
                                if (errors1[1] != 1)
                                    builder.AppendLine("도어 열림", true);
                                else
                                    cnt--;
                                break;
                            case 3:
                                builder.AppendLine("열풍 팬 에러", true);
                                break;
                            case 4:
                                builder.AppendLine("열풍 히터 과열", true);
                                break;
                            case 5:
                                builder.AppendLine("열풍 히터 오픈", true);
                                break;
                            case 6:
                                builder.AppendLine("만수, 워터센서 오픈", true);
                                break;
                            case 7:
                                builder.AppendLine("열풍 히터 저온", true);
                                break;
                        }
                    }
                }
            return builder.ToString();
        }

        private string getModelName(int model)
        {
            switch (model)
            {
                case 0:
                    return "PCS 400";
                case 1:
                    return "PCS 500";
                case 2:
                    return "PCS 350";
                default:
                    return "";
            }
        }

        private int getMotorValue(int run)
        {
            Console.WriteLine("Value:{0}", run);
            switch (run)
            {
                case 2:
                    return 75;
                case 3:
                    return 75;
                case 4:
                    return 25;
                case 5:
                    return 25;
                case 8:
                    return 50;
                case 9:
                    return 50;
                default:
                    return 0;
            }
        }

        private string getMotorState(int run)
        {
            switch (run)
            {
                case 2:
                    return "CW";
                case 3:
                    return "CW";
                case 4:
                    return "CCW";
                case 5:
                    return "CCW";
                case 8:
                    return "STOP";
                case 9:
                    return "STOP";
                default:
                    return "";
            }
        }
    }
}
