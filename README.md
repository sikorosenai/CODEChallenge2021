# Daily Coding Languages App

A mobile app which provides daily facts about a coding language, as well as asking a question about said language that the user can attempt. 

GitHub repository: https://github.com/sikorosenai/DailyLanguagesApp

Building:
This app was built with Visual Studio Community Edition 2019, with Xamarin and mobile .NET development components installed. The Android build has been tested with the emulator and on a Nokia Android device. The iOS build has not been tested as of yet. To run the application on the emulator, first download the repository from github, then when the solution is loaded right-click the DailyCodingLanguagesApp.Android project and set it as the startup project. The solution can be run and should load immediately on an emulator.  

Alternatively, the enclosed APK file can be copied onto an android device if it is connected to a computer with USB, or downloaded from the github site using an android phone, using the link:
 
https://github.com/sikorosenai/DailyLanguagesApp/suites/5113086008/artifacts/153375607

The latest APK can also be found from the github action latest build:
https://github.com/sikorosenai/DailyLanguagesApp/actions/workflows/builds.yml

Packages used via NuGet:
Newtonsoft Json for parsing web downloads
Plugin.LocalNotification for displaying diconnected popup 
