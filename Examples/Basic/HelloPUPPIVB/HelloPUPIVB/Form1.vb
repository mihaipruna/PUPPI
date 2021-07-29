
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports PUPPIModel
'this is required for ArrayList
Imports System.Collections
'*******************************************************************************************************************
'PUPPI Hello World Sample for C#
'A basic PUPPI GUI environment for Math operations
'Output can be saved to files
'In the PUPPI GUI, you can load examples of visual programs from the Debug\Examples folder.
'http://visualprogramminglanguage.com
'Advanced project samples are available to PUPPI subscribers. Contact us at sales@pupi.co
'*******************************************************************************************************************


Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'this is needed at the beginning even if no settings are changed
        PUPPIGUI.PUPPIGUISettings.initializeSettings()
        'creates and adds the PUPPI canvas to one tab page
        Dim pmc As New PUPPIGUIController.PUPPIProgramCanvas(1000, 460)
        PUPPIGUIController.FormTools.AddPUPPIProgramCanvastoForm(pmc, Me, 5, 100)
        'math modules menu
        Dim mathbuttonsmenu As New PUPPIGUIController.PUPPIModuleKeepMenu("Math Functions", 200, 80, 90, 20, 2)
        'add the premade PUPPI Math Modules
        mathbuttonsmenu.AddPUPPIPremadeMathModules()
        'also add functions from the System.Math class
        mathbuttonsmenu.AddMenuButtonList(PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(GetType(System.Math)))
        'add the menu to the form
        PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(mathbuttonsmenu, Me, 0, 20)
        'logic modules menu
        Dim logicmenubuttons As New PUPPIGUIController.PUPPIModuleKeepMenu("Logic", 60, 80, 50, 20, 1)
        logicmenubuttons.AddPUPPIPremadeLogicModules(0.2, 0.9, 0.5, 1)
        logicmenubuttons.AddMenuButton(TryCast(New PUPPINOT(), PUPPIModule), 0.2, 0.9, 0.5, 1)
        PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(logicmenubuttons, Me, 200, 20)
        'list modules menu
        Dim listmenubuttons As New PUPPIGUIController.PUPPIModuleKeepMenu("List Ops", 100, 80, 80, 20, 1)
        listmenubuttons.AddPUPPIPremadeListModules(0.1, 0.9, 0.7, 1)
        PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(listmenubuttons, Me, 260, 20)
        'data input modules menu
        Dim datainputmenubuttons As New PUPPIGUIController.PUPPIModuleKeepMenu("Input", 90, 80, 70, 20, 1)
        datainputmenubuttons.AddPUPPIPremadeDataInputModules(0.1, 0.9, 0.8, 1)
        'add string input with default colors
        datainputmenubuttons.AddMenuButton(New PUPPIStringInput())
        PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(datainputmenubuttons, Me, 360, 20)
        'new menu
        Dim outputmenubuttons As New PUPPIGUIController.PUPPIModuleKeepMenu("Output", 90, 80, 70, 20, 1)
        'add file output module with default colors for button
        outputmenubuttons.AddMenuButton(New PUPPIWriteCSV())
        'add module created automatically from the SimpleConcatStrings method below
        Dim methodparmtypes As New List(Of Type)()
        Dim typer As String = ""
        'add the two type String objects to the list to match the parameters of the SampleConcatStrings method and retrieve it to convert to PUPPIModule
        methodparmtypes.Add(typer.[GetType]())
        methodparmtypes.Add(typer.[GetType]())
        Dim mysamplestrings As New SampleStrings
        Dim PUPPIscs As Type = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(mysamplestrings.GetType(), "SampleConcatStrings", methodparmtypes)
        outputmenubuttons.AddMenuButton(TryCast(System.Activator.CreateInstance(PUPPIscs), PUPPIModule))
        PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(outputmenubuttons, Me, 450, 20)

        'file dropdown menu
        Dim pdmfile As New PUPPIGUIController.PUPPIDropDownMenu(pmc, "File")
        pdmfile.addStandardFileMenuOptions()
        PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmfile, Me)
        'edit dropdown menu
        Dim pdmedit As New PUPPIGUIController.PUPPIDropDownMenu(pmc, "Edit")
        pdmedit.addStandardEditMenuOptions()
        PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmedit, Me, 40, 0)
        'view dropdown menu
        Dim pdmview As New PUPPIGUIController.PUPPIDropDownMenu(pmc, "View")
        pdmview.addStandardViewMenuOptions()
        PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmview, Me, 80, 0)
        'help dropdown menu
        Dim pdmhelp As New PUPPIGUIController.PUPPIDropDownMenu(pmc, "Help")
        pdmhelp.addStandardHelpMenuOptions(Uri.UnescapeDataString(New Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath) + "/PUPPI-user-help.chm")
        PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmhelp, Me, 120, 0)
    End Sub
End Class
'custom module: Logical NOT
Public Class PUPPINOT
    Inherits PUPPIModule
    Public Sub New()
        MyBase.New()
        'required : call the parent constructor first
        'module's name
        name = "NOT"
        'one output defaulted to 0
        outputs.Add(0)
        outputnames.Add("Result")
        'description to show up when hovering over module in menu
        description = "NOT Operation"
        'one input
        inputnames.Add("Input")

        inputs.Add(New PUPPIInParameter())
    End Sub
    'this function controls what the module does when visual program is running
    Public Overrides Sub process_usercode()


        Dim b1 As Boolean = False

        Try

            b1 = Convert.ToBoolean(usercodeinputs(0))
        Catch
            usercodeoutputs(0) = 0
            Return
        End Try
        usercodeoutputs(0) = Convert.ToInt16(Not b1)




        Return
    End Sub
End Class
'custom module: string input
Public Class PUPPIStringInput
    Inherits PUPPIModule
    'constructor
    Public Sub New()
        MyBase.New()
        'needs to call base constructor

        name = "String input"
        outputs.Add(" ")
        outputnames.Add("Value")
    End Sub
    'value is set when user double clicks on node
    Public Overrides Sub doubleClickMe_userCode(clickXRatio As Double, clickYRatio As Double, clickZRatio As Double)
        MyBase.doubleClickMe_userCode(clickXRatio, clickYRatio, clickZRatio)
        Dim entered As String = ""
        entered = InputBox("Please enter a string", "Enter text:", entered)
        usercodeoutputs(0) = entered

    End Sub


End Class
'custom module: writes number,list of numbers or grid (arraylist of arraylists) to a file
Public Class PUPPIWriteCSV
    Inherits PUPPIModule
    Public Sub New()
        MyBase.New()
        name = "Write CSV"
        description = "writes number,list of numbers or grid (arraylist of arraylists) to a comma separated text file"
        'for output, we will display a message 
        outputs.Add("not-run")
        outputnames.Add("Result")
        'multiple types of variables will be accounted for
        inputnames.Add("Item/ArrayList/Grid")
        inputs.Add(New PUPPIInParameter())
        'also the complete file path as string
        inputnames.Add("Output file path")
        inputs.Add(New PUPPIInParameter())
        'we will access inputs and outputs directly
        completeprocessoverride = True
    End Sub
    'file will get overwritten every time the program runs
    Public Overrides Sub process_usercode()

        'if failure of any way, we report it
        'for instance, if not all inputs are connected, etc.
        'with completeprocessoverride enabled we need to do our own error checking
        Try
            'get the path. we reference the connected output directly
            Dim fpath As String = inputs(1).[module].outputs(inputs(1).outParIndex).ToString()
            'check list or grid input
            If TypeOf inputs(0).[module].outputs(inputs(0).outParIndex) Is ArrayList Then
                Dim orlist As ArrayList = TryCast(inputs(0).[module].outputs(inputs(0).outParIndex), ArrayList)
                '2d grid input (Arraylist of arraylists)
                If TypeOf orlist(0) Is ArrayList Then
                    'store all rows in a string array
                    Dim rcount As Integer = orlist.Count
                    Dim allrows As String() = New String(rcount - 1) {}
                    For rc As Integer = 0 To rcount - 1
                        Dim myrow As ArrayList = TryCast(orlist(rc), ArrayList)
                        Dim ccount As Integer = myrow.Count
                        'write row values separated by comma to array
                        Dim rowstring As String = ""
                        For cc As Integer = 0 To ccount - 1
                            If cc <> ccount - 1 Then
                                rowstring += myrow(cc).ToString() + ","
                            Else
                                rowstring += myrow(cc).ToString()

                            End If
                        Next
                        allrows(rc) = rowstring
                    Next


                    System.IO.File.WriteAllLines(fpath, allrows)
                Else
                    'one dimensional generic list
                    Dim singlerow As String = ""
                    For i As Integer = 0 To orlist.Count - 1
                        If i <> orlist.Count - 1 Then
                            singlerow += orlist(i).ToString() + ","
                        Else
                            singlerow += orlist(i).ToString()
                        End If
                    Next
                    System.IO.File.WriteAllText(fpath, singlerow)
                End If
            Else
                'no matter what the input is we convert it to string
                Dim text As String = inputs(0).[module].outputs(inputs(0).outParIndex).ToString()
                ' WriteAllText creates a file, writes the specified string to the file, 
                ' and then closes the file.
                System.IO.File.WriteAllText(fpath, text)
            End If



            outputs(0) = "success"
        Catch
            outputs(0) = "error"
            Return
        End Try





        Return
    End Sub


End Class
'sample class to show how PUPPI can create Visual Programming Modules from existing methods
Public Class SampleStrings
    'simple method concatenating two strings
    Public Shared Function SampleConcatStrings(s1 As String, s2 As String) As String
        Return s1 & s2
    End Function
End Class







