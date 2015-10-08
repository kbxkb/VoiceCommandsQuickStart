using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VoiceCommandsQuickStart
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AnswersPage : Page
    {
        public AnswersPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            SpeechRecognitionResult speechResult = e.Parameter as SpeechRecognitionResult;

            questionTextBox.Text = speechResult.Text;
            answerTextBox.Text = "There are 50 states and 1 federal district in the US";

            var synthesizer = new SpeechSynthesizer();
            SpeechSynthesisStream synthesisStream = await
                                  synthesizer.SynthesizeTextToStreamAsync(answerTextBox.Text);

            // Set the source and start playing the synthesized audio stream.
            media.AutoPlay = true;
            media.SetSource(synthesisStream, synthesisStream.ContentType);
            media.Play();
        }
    }
}
