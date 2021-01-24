# WPF_RestAPI_Test
This is a project on  to sync data from REST API into your LocalDB using WPF application.

Listed below is the tools that are used for this development:
- Microsoft Visual Studio Community 2019 (Version 16.8.4)
- SQL Server Management Studio V 18.6

Flow of this application goes as:
1. Enter the username and password into the textbox in MainWindow and click 'Login'.
2. The event of clicking button 'Login' will authenticate and return a bearer token and GetPlatformWellActual() will be executed automatically.
3. GetPlatformWellActual() will call REST API to retrieve platform and well values and insert into the LocalDB.
4. The application will then open a new window which consists of 2 buttons. 
5. "Rerun GetPlatformWellActual()" button is used to re-execute GetPlatformWellActual() while "Run GetPlatformWellDummy()" button is used to test the application if missing or new key to be added.
