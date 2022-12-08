using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Serial_protocol.ViewModel
{
    //응용 프로그램의 모든 ViewModel 클래스에 대한 기본 클래스입니다.
    //속성 변경 통지에 대한 지원을 제공하며 DisplayName 속성을 가지고 있습니다.
    //이 수업은 추상적이다,
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        #region Constructor
        protected ViewModelBase()
        {

        }
        #endregion // Constructor


        #region DisplayName
        //이 개체의 사용자 친화적인 이름을 반환합니다.자식 클래스는 이 속성을 새 값으로 설정하거나 재정의하여 주문형 값을 결정할 수 있습니다.
        public virtual string DisplayName { get; protected set; }
        #endregion // DisplayName


        #region Debugging Aides
        // 이 개체에 지정된 이름의 공용 속성이 없는 경우 개발자에게 경고합니다.이 메서드는 릴리스 빌드에 없습니다.
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // 속성 이름이 이 개체의 실제 공용 인스턴스 속성과 일치하는지 확인합니다.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }
        //예외를 발생시킬지 또는 디버그할지를 반환합니다.
        //실패() 는 잘못된 속성 이름을 VerifyPropertyName 메서드에 전달할 때 사용됩니다.
        //기본값은 false이지만 단위 테스트에 사용되는 하위 클래스가 이 속성의 게터를 재정의하여 true를 반환할 수 있습니다.
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion // Debugging Aides


        #region INotifyPropertyChanged Members

        /// <summary>
        /// 이 개체의 속성에 새 값이 있을 때 발생합니다.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 이 개체의 PropertyChanged 이벤트를 발생시킵니다.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // INotifyPropertyChanged Members


        #region IDisposable Members

        //이 개체가 응용 프로그램에서 제거되고 가비지 수집 대상이 될 때 호출됩니다.
        public void Dispose()
        {
            this.OnDispose();
        }

        /// 하위 클래스는 이벤트 핸들러 제거와 같은 정리 논리를 수행하기 위해 이 메서드를 재정의할 수 있습니다.
        protected virtual void OnDispose()
        {
        }

#if DEBUG
        /// ViewModel 개체가 가비지 수집이 제대로 되도록 하는 데 유용합니다.
        ~ViewModelBase()
        {
            string msg = string.Format("{0} ({1}) ({2}) Finalized", this.GetType().Name, this.DisplayName, this.GetHashCode());
            System.Diagnostics.Debug.WriteLine(msg);
        }
#endif

        #endregion // IDisposable Members
    }
}