using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;

namespace Alltech.Utils
{
    public class AlertDialog
    {
        private string _Title = "";
        private string _Message = "";

        private AlertButton _PositiveButton;
        private AlertButton _NeutralButton;
        private AlertButton _NegativeButton;

        public delegate void OnButtonClick();

        public AlertDialog()
        {

        }

        public AlertDialog(string message)
        {
            _Message = message;
        }

        public AlertDialog SetTitle(string title)
        {
            _Title = title;

            return this;
        }

        public AlertDialog SetMessage(string message)
        {
            _Message = message;

            return this;
        }

        public AlertDialog SetButton(DialogButtonInterface whichButton, string text, OnButtonClick onButtonClick)
        {
            AlertButton alertButton = new AlertButton(whichButton, text, onButtonClick);
            
            switch(whichButton)
            {
                case DialogButtonInterface.BUTTON_POSITIVE:
                    _PositiveButton = alertButton;
                    break;
                default:
                case DialogButtonInterface.BUTTON_NEUTRAL:
                    _NeutralButton = alertButton;
                    break;
                case DialogButtonInterface.BUTTON_NEGATIVE:
                    _NegativeButton = alertButton;
                    break;
            }

            return this;
        }

        public MessageDialog Build()
        {
            MessageDialog messageDialog;

            if(!string.IsNullOrWhiteSpace(_Title))
            {
                messageDialog = new MessageDialog(_Message, _Title);
            } else
            {
                messageDialog = new MessageDialog(_Message);
            }

            messageDialog.Commands.Clear();

            if(_PositiveButton != null)
            {
                messageDialog.Commands.Add(_PositiveButton.Build(CommandHandlers));
            }
            if(_NegativeButton != null)
            {
                messageDialog.Commands.Add(_NegativeButton.Build(CommandHandlers));
            }
            if(_NeutralButton != null)
            {
                messageDialog.Commands.Add(_NeutralButton.Build(CommandHandlers));
            }
            
            return messageDialog;
        }

        public IAsyncOperation<IUICommand> ShowAsync()
        {
            return Build().ShowAsync();
        }

        public void CommandHandlers(IUICommand commandLabel)
        {
            OnButtonClick onButtonClick = commandLabel.Id as OnButtonClick;

            if(onButtonClick != null)
            {
                onButtonClick();
            }
        }


        
        

        private class AlertButton
        {
            private DialogButtonInterface _WhichButton;
            private string _Text;
            private OnButtonClick _OnButtonClick;

            public DialogButtonInterface WhichButton
            {
                get; set;
            }

            public string Text
            {
                get; set;
            }

            public OnButtonClick OnButtonClick
            {
                get; set;
            }

            public AlertButton(DialogButtonInterface whichButton, string text, OnButtonClick onButtonClick)
            {
                WhichButton = whichButton;
                Text = text;
                OnButtonClick = onButtonClick;
            }

            public UICommand Build(UICommandInvokedHandler UICommandInvokedHandler) 
            {
                return new UICommand
                {
                    Label = Text,
                    Invoked = UICommandInvokedHandler,
                    Id = OnButtonClick,
                };
            }
        }

        public enum DialogButtonInterface
        {
            BUTTON_POSITIVE,
            BUTTON_NEUTRAL,
            BUTTON_NEGATIVE
        }
    }
}
