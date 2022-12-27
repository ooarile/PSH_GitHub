using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practice._15_Winform
{
    public partial class SplitContainerForm : Form
    {

        /* SplitContainer의 Dock으로 위치 설정*/
        public SplitContainerForm()
        {
            InitializeComponent();                 
        }

        #region Resize, Layout
        /*
        * 폼 크기 임의의 변경 (Forms.Control)
        * Resize 발생 시점 
        *      컨트롤의 크기를 변경하면 발생
        * Resize 사용 용도
        *      일정한 크기 유지
        */
        private void SplitContainerForm_Resize(object sender, EventArgs e)
        {


            // 둘 다 가능
            /*
            Control control = (Control)sender;
            control.Width = 500;
            control.Height = 500;
            */

            Width = 500;
            Height = 500;

        }

        /*
         * Layout 발생 시점
         *      폼이 생성되기 직전에 호출
         *      폼의 크기 변경
         *      폼의 컨트롤의 배치가 바뀌는 경우 --> 버튼 클릭 등도 가능
         * Resize vs Layout 역할 비교
         *      Resize < Layout
         */
        private void SplitContainerForm_Layout(object sender, LayoutEventArgs e)
        {

        }
        #endregion


        /*
         * System.Windows.Forms.Timer
         * 타이머 이벤트 제한 사항
         *      1초에 100회 이하 발생 --> 보장성 없음
         * 타이머 이벤트 설정 순서
         *      1. 타이머 컨트롤 폼에 설정
         *          도구상자-->timer폼에 놓기
         *      2. 타이머 컨트롤 속성 변경
         *          Enabled --> true
         *          interval --> n[ms] 발생되도록 설정
         *      3. Timer event에서 Tick 선택     
         */
        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
