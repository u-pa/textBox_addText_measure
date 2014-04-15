using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace textBox_addText_measure
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ログ記録
        void writeLog(String logText)
        {
            textBox_result.SelectionStart = textBox_result.Text.Length;
            textBox_result.SelectionLength = 0;
            textBox_result.SelectedText = logText + "\r\n";
        }

        // 測定タイプ
        public enum MeasureType
        {
            Text,               // テキストボックスのテキストにテキストボックスのテキスト + 追加のテキストを代入する方法
            Selection,          // テキストボックスの Selection にテキストを挿入する方法
            AppendText,         // AppendText メソッドを使用する方法
        };

        // テキスト追加処理を実行し、処理時間を測定する
        private TimeSpan measure(MeasureType measureType, bool doEvents, int MaxLoop)
        {
            textBox1.Text = "";
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < MaxLoop; i++)
            {
                String s = i + ":" + testText + "\r\n";
                switch (measureType)
                {
                    case MeasureType.Text:
                        textBox1.Text = textBox1.Text + s;
                        break;
                    case MeasureType.Selection:
                        textBox1.SelectionStart = textBox1.Text.Length;
                        textBox1.SelectionLength = 0;
                        textBox1.SelectedText = s;
                        break;
                    case MeasureType.AppendText:
                        textBox1.AppendText(s);
                        break;
                }

                if (doEvents)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
            }

            sw.Stop();

            return sw.Elapsed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            const int MaxLoop = 1000;
            MeasureType measureType = (MeasureType)comboBox1.SelectedItem;
            bool doEvents = checkBox1.Checked;

            // 処理時間のブレがないことを確認するため、何度かループする
            for (int i = 1; i <= 3; i++)
            {
                TimeSpan Elapsed = measure(measureType, doEvents, MaxLoop);

                TimeSpan average = new TimeSpan(Elapsed.Ticks / MaxLoop);
                writeLog("測定タイプ=," + measureType + ", DoEvents=," + doEvents + ", " + i + "回目, 処理時間合計=," + Elapsed + ", 1回あたりの処理時間=," + average);
            }
        }

        String testText = "";   // テストに使用する文字列
        private void Form1_Load(object sender, EventArgs e)
        {
            // 1000 文字分のテキストを作成
            for (int i = 0; i < 1000; i++)
            {
                testText += (i % 10).ToString();
            }

            comboBox1.DataSource = Enum.GetValues(typeof(MeasureType));
        }
    }
}
