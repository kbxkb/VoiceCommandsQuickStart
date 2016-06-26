# VoiceCommandsQuickStart
Extending Cortana with Voice Commands
Quick Start Challenge – //Build 2015
Overview
Windows 10 allows developers to extend Cortana’s functionality to enable users to input a one-utterance command directed at an app. Cortana enables applications to handle voice commands by launching the application in either the foreground or the background. Voice commands that require additional context or user input are best handled in the foreground, whereas commands that are simple enough can be handled in the background.
What You’ll Learn
•	How to enable voice commands that launch your application from Cortana
•	How to enable voice commands that interact with the user on Cortana’s canvas
The Challenge
We will start by creating a new project in Visual Studio 2015 CTP – launch Visual Studio from the task bar or the Start Menu.

Make a New Project

From the File Menu, select New Project.
 
Using the New Project window, create a Blank Universal App from the provided project templates. 
Expand the template tree on the left-hand side of the menu to get to the Windows Universal templates for C#. They are located under Installed → Templates → Visual C# → Windows Universal.
Then select a Blank Application. A blank UAP application is a project for the simplest UAP appFor the purpose of this lab, name the project VoiceCommandsQuickStart.

Creating the Voice Command Definition File
From within the Solution Explorer, right click on the project, VoiceCommandsQuickStart (unless you named it differently). From the Add menu, select New Item.
 
In the left hand pane, click Data and select XML file. For the purpose of this lab, name the file VoiceCommandsQuickStart.xml. Click the Add button.
 
In the Solution Explorer, click on the VoiceCommandsQuickStart.xml. In the Properties pane, set Copy to Output Directory to Copy Always. This will assure that the XML file is part of the application package.
 
Edit the VoiceCommandsQuickStart.xml to define the commands the application will handle. For the purpose of this lab, the app will define a command that recognizes this input: “How many states are in the US?”.
<?xml version="1.0" encoding="utf-8"?>
<VoiceCommands xmlns="http://schemas.microsoft.com/voicecommands/1.2">
	<CommandSet xml:lang="en-us" Name="CommandSet_en-us">
		<CommandPrefix> Quick Start, </CommandPrefix>
		<Example> Ask me something </Example>

      <Command Name="HowManyStates">
      <Example> How many states in the US? </Example>
      <ListenFor> How many states [are] in [the] US </ListenFor>
      <ListenFor> How many states [are] in the United States </ListenFor>
      <Feedback> Looking for an answer </Feedback>
      <Navigate/>
    </Command>

  </CommandSet>

</VoiceCommands>
Notice that the XML sets a CommandPrefix , in this case it is Quick Start. This is the prefix that the user needs to use when invoking the voice command. When invoking the voice command, the user needs to say to Cortana “Quick Start, How many states are in US?”. Notice that the language for this command is set to en-us.
Press Ctrl+S to save the VoiceCommandsQuickStart.xml file once you copy the XML snippet provided above.

Register the Voice Command Definition file 

In the Solution Explorer, click on the App.xaml and expand. Click on the App.xaml.cs to open the file.
  
Add this namespace at the top of the file.
using Windows.ApplicationModel.VoiceCommands;
using Windows.Media.SpeechRecognition;

Find the OnLaunched method that was added for you when the Project was created. At the very end of the method, add the code that registers the VoiceCommandsQuickStart.xml with the System. Make sure to declare OnLauched method as async as shown below.
protected override async void OnLaunched(LaunchActivatedEventArgs e)
{

    // add this at the very end of the method 
    var storageFile = await   
    Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
    new Uri("ms-appx:///VoiceCommandsQuickStart.xml"));

    await   
    VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync
                                                                    (storageFile);
 }
                

Press Ctrl+S to save the App.xaml.cs file.
Create a new page
From within the Solution Explorer, right click on the project, VoiceCommandQuickStart (unless you named it differently). From the Add menu, select New Item.
Find the BlankPage template under Installed -> Visual C# -> Blank Page a name it AnswersPage.xaml.
 

Handle the activation-by-voice-command
In the App.xaml.cs file, override the OnActivated method. This method is called when the application is launched through voice from Cortana.
Add this code snippet to the file. Notice how the code checks that the activation kind is ‘voice’ , it casts the event arguments to VoiceCommandActivatedEventArgs to get access to the recognized command. It then inspects the name of the recognized command and it navigates to the Details page.
protected override void OnActivated(IActivatedEventArgs e)
{

    Frame rootFrame = Window.Current.Content as Frame;

    if (rootFrame == null)
    {
        rootFrame = new Frame();
        rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

        rootFrame.NavigationFailed += OnNavigationFailed;

        Window.Current.Content = rootFrame;
    }

    if (e.Kind == Windows.ApplicationModel.Activation.ActivationKind.VoiceCommand)
    {
        var commandArgs = e as 
        Windows.ApplicationModel.Activation.VoiceCommandActivatedEventArgs;
        SpeechRecognitionResult speechRecognitionResult = commandArgs.Result;

        string voiceCommandName = speechRecognitionResult.RulePath[0];

        if (voiceCommandName == "HowManyStates")
        {
            rootFrame.Navigate(typeof(AnswersPage), speechRecognitionResult);
        }
    }

    if (rootFrame.Content == null)
    {
        rootFrame.Navigate(typeof(MainPage), null);
    }

    Window.Current.Activate();
}
 
Press Ctrl+S to save the App.xaml.cs file.
Show and speak an answer to the voice command
In the Solution Explorer, click on the AnswersPage.xaml to open and copy this snippet. This will add two text boxes that will show the voice command and the answer and a MediaElement used to speak the answer.
<Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">

<TextBlock x:Name="pageTitle" Text="Answer Page" Style="{StaticResource HeaderTextBlockStyle}" TextWrapping="NoWrap" VerticalAlignment="Bottom" Margin="0,0,30,1830"/>

<TextBox x:Name="questionTextBox" HorizontalAlignment="Left" Height="72" Margin="10,239,0,0" TextWrapping="Wrap" Text="Question:" VerticalAlignment="Top" Width="485"/>

<TextBox x:Name="answerTextBox" HorizontalAlignment="Left" Height="72" Margin="10,136,0,0" TextWrapping="Wrap" Text="Answer:" VerticalAlignment="Top" Width="485"/>

<MediaElement x:Name="media" AutoPlay="False"/>
</Grid>
Press Ctrl+S to save.

In the Solution Explorer, click on the AnswersPage.xaml.cs to open.
Copy this snippet to add these namespaces at the top of the file.
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
Copy 
protected async override void OnNavigatedTo(NavigationEventArgs e)
{
    SpeechRecognitionResult speechResult = e.Parameter as SpeechRecognitionResult;

    questionTextBox.Text = speechResult.Text;
    answerTextBox.Text = "There are 50 states and one federal district in US.";

    var synthesizer = new SpeechSynthesizer();
    SpeechSynthesisStream synthesisStream = await
                          synthesizer.SynthesizeTextToStreamAsync(answerTextBox.Text);

    // Set the source and start playing the synthesized audio stream.
    media.AutoPlay = true;
    media.SetSource(synthesisStream, synthesisStream.ContentType);
    media.Play();

}
Press Ctrl+S to save.

Try it out 
Lastly, we need to run the project. Using the debug menu pictured below, you can use the chevron to change your target platforms. Try running this same binary on your Windows 10 Technical Preview machine along with the Windows Phone 10 Technical Preview device.
 
Run the application at least once to register the voice command. Minimize or close the application.
Open Cortana and press the microphone button.
Say: “Quick Start, how many states are in US?”
You should see this screen in Cortana:
 
The app should launch on the AnswersPage and you should see and hear: 
“There are 50 states and one federal district in US”.
Great, you now enabled a voice command that launches an app in the foreground. The app presents UI with the answer and uses Speech Synthesis to speak the answer.
Let change the voice command to be handled by the app in the background and use Cortana’s canvas and screen to present the answer to the user.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
