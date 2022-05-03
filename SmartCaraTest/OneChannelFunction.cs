using SmartCaraTest.data;
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
        string now = DateTime.Now.ToString("yyyy-MM-dd");
        
        public string GetDateTime()
        {
            DateTime NowDate = DateTime.Now;
            return NowDate.ToString("yyyy-MM-dd HH:mm:ss") + ":" + NowDate.Millisecond.ToString("000");
        }

        private void PrintCommand(byte[] command)
        {
            string hex = "";
            foreach (byte b in command)
            {
                hex += " " + b.ToString("X2");
            }
            Console.WriteLine("Length:{0}, Data:{1}",command.Length, hex);
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
            PrintCommand(array);
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

            int t_hour = data[18];
            int t_min = data[19];
            int t_sec = data[20];
            int runTime = data[21];
            string time = string.Format("{0:D2}:{1:D2}", minute, second);
            string t_time = string.Format("{0:D2}:{1:D2}:{2:D2}", t_hour, t_min, t_sec);
            byte[] current = { 0, 0, data[13], data[14] };
            Array.Reverse(current);
            int currentInt = BitConverter.ToInt32(current, 0) * 10;
            double currnetDouble = (double)currentInt / 1000.0;
            int year = data[51];
            int month = data[52];
            int day = data[53];
            int version = data[54];
            string micom = $"{year}.{month}.{day}. ver {version}";
            byte error0 = data[49];
            byte error1 = data[50];
            int[] binary0 = Enumerable.Range(1, 8).Select(i => error0 / (1 << (8 - i)) % 2).ToArray();
            int[] binary1 = Enumerable.Range(1, 8).Select(i => error1 / (1 << (8 - i)) % 2).ToArray();
            Array.Reverse(binary0);
            Array.Reverse(binary1);
            //5 = 0101 RUN CCW
            //9 = 1001 RUN STOP
            //3 = 0011 RUN CW
            //8 = 1000 STOP 
            //2 = 0010 CW
            //4 = 0100 CCW
            bool[] errors0 = new bool[8];
            bool[] errors1 = new bool[8];
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
            int heater_setting_temp = data[26];
            int heater_setting_offtime = data[27];
            int fan_duty = data[29];
            int hot_air_fan_duty = data[30];
            if (channel.run)
            {
                ReadData read = new ReadData()
                {
                    date = now,
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
            DetailView.temper.Value3.Content = 0;
            DetailView.temper.Value4.Content = fan_duty;

            DetailView.heater.Value1.Content = 0;
            DetailView.heater.Value2.Content = hot_air_fan_duty;

            
            channel.Item21.cont.Content = getModelName(data[48]);
            channel.Item22.cont.Content = micom;
            channel.Item24.cont.Content = time;
            channel.Item1.cont.Content = heatertemp + "ºC";
            channel.Item2.cont.Content = airtemp + "ºC";
            channel.Item3.cont.Content = airheatertemp + "ºC";
            channel.Item11.cont.Content = heateroff;
            channel.Item12.cont.Content = airaverage + "ºC";
            channel.Item13.cont.Content = heaterduty;
            channel.Item14.cont.Content = currnetDouble + "A";
            channel.Item6.cont.Content = t_time;
            channel.Item4.Title = getMotorState(motorRun);
            channel.Item4.cont.Content = motorRunTime.ToString() + "s";
            channel.Item26.cont.Content = (mode + 1).ToString();
            if (channel.run)
            {
                if (channel.Item1Check.IsChecked.Value)
                {
                    channel.list1.Add(new KeyValuePair<DateTime, int>(DateTime.Now, heatertemp));
                }
                if (channel.Item2Check.IsChecked.Value)
                {
                    channel.list2.Add(new KeyValuePair<DateTime, int>(DateTime.Now, airtemp));
                }
                if (channel.Item3Check.IsChecked.Value)
                {
                    channel.list3.Add(new KeyValuePair<DateTime, int>(DateTime.Now, airheatertemp));
                }
                if (channel.Item4Check.IsChecked.Value)
                {
                    channel.list4.Add(new KeyValuePair<DateTime, int>(DateTime.Now, getMotorValue(motorRun)));
                }
                if (channel.Item5Check.IsChecked.Value)
                {
                    channel.list5.Add(new KeyValuePair<DateTime, int>(DateTime.Now, heateroff));
                }
                if (channel.Item6Check.IsChecked.Value)
                {
                    channel.list6.Add(new KeyValuePair<DateTime, int>(DateTime.Now, airaverage));
                }
                if (channel.Item7Check.IsChecked.Value)
                {
                    channel.list7.Add(new KeyValuePair<DateTime, int>(DateTime.Now, heaterduty));
                }
                if (channel.Item8Check.IsChecked.Value)
                {
                    channel.list8.Add(new KeyValuePair<DateTime, double>(DateTime.Now, currnetDouble));
                }
            }
        }
        private string getModelName(int model)
        {
            switch (model)
            {
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
            switch (run)
            {
                case 2:
                    return 75;
                case 3:
                    return 75;
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
