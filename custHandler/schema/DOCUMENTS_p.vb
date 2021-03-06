Imports PriPROC6.Interface.oData
Imports System.Reflection

''' <summary>
'''
''' This code was generated by the Schema Utility.
'''
''' Form: DOCUMENTS_p as List(Of rowDOCUMENTS_p)
''' Date: 06/05/2019
'''
''' </summary>
<oFormClass("rowDOCUMENTS_p", "CUSTPERSONNEL")>
Public Class DOCUMENTS_p : Inherits oForm

    ''' <summary>
    ''' DOCUMENTS_p Constructor method.
    ''' </summary>
	''' <param name="Sender">The Assembly.GetExecutingAssembly of your project</param>
    ''' <param name="Parent">Optional: Parent oRow of this form</param>
    Sub New(byref Sender As Assembly, Optional Parent As oRow = Nothing)
        MyBase.New(Sender, Parent)

    End Sub

    ''' <summary>
    ''' Add a new row to the DOCUMENTS_p form.
    ''' </summary>
    ''' <returns>rowDOCUMENTS_p</returns>
    Public Function AddRow() As rowDOCUMENTS_p
        With Me
            .Add(New rowDOCUMENTS_p(Me))
            Return .Last

        End With

    End Function

End Class

''' <summary>
''' Defines rows in the DOCUMENTS_p form.
''' </summary>
<oRowClass("DOCUMENTS_p", "serialDOCUMENTS_p")>
Public Class rowDOCUMENTS_p : Inherits oRow

    ''' <summary>
    ''' rowDOCUMENTS_p Constructor method.
    ''' </summary>
    ''' <param name="Parent">Parent oForm of this row</param>
    Sub New(Parent As oForm)
        MyBase.New(Parent)

    End Sub
    
	''' <summary>
    ''' Get/set the CUSTPERSONNEL subform.
    ''' </summary>
    ''' <returns>CUSTPERSONNEL</returns>
	Public Property CUSTPERSONNEL As CUSTPERSONNEL
        Get
            Return MyBase.SubForms("CUSTPERSONNEL")
        End Get
        Set(value As CUSTPERSONNEL)
            MyBase.SubForms("CUSTPERSONNEL") = value
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Customer Number" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Customer Number", mandatory:=True, length:=16)>
    Public Property CUSTNAME As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get the Read Only "Customer Name" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Customer Name", ReadOnly:=True, length:=48)>
    Public Readonly Property CUSTDES As String
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get / Set the "Project Number" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Project Number", Key:=True, length:=16)>
    Public Property DOCNO As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Project Description" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Project Description", length:=32)>
    Public Property PROJDES As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Description (Lang2)" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Description (Lang2)", length:=32)>
    Public Property EPROJDES As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Project Manager" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Project Manager", mandatory:=True, length:=20)>
    Public Property MUSERLOGIN As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Project Supervisor" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Project Supervisor", length:=20)>
    Public Property SUSERLOGIN As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Billable?" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Billable?", length:=1)>
    Public Property FLAG As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Status" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Status", mandatory:=True, length:=12)>
    Public Property STATDES As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Project Type" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Project Type", length:=3)>
    Public Property PROJTYPECODE As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get the Read Only "Project Type Descrip" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Project Type Descrip", ReadOnly:=True, length:=16)>
    Public Readonly Property PROJTYPEDES As String
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get / Set the "Add'l Classification" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Add'l Classification", length:=3)>
    Public Property PROJTYPECODE2 As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get the Read Only "Classif. Description" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Classif. Description", ReadOnly:=True, length:=16)>
    Public Readonly Property PROJTYPEDES2 As String
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Cust. Group Code" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Cust. Group Code", ReadOnly:=True, length:=2)>
    Public Readonly Property CTYPECODE As String
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Group Description" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Group Description", ReadOnly:=True, length:=32)>
    Public Readonly Property CTYPENAME As String
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get / Set the "Date Opened" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Date</returns>	
    <oDataColumn("Date Opened", mandatory:=True)>
    Public Property CURDATE As Date
        Get
            Return getProperty()
        End Get
        Set(value As Date)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Automatic Cost Calc?" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Automatic Cost Calc?", length:=1)>
    Public Property AUTOVERCALC As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Installation Date" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Date</returns>	
    <oDataColumn("Installation Date")>
    Public Property INSTDATE As Date
        Get
            Return getProperty()
        End Get
        Set(value As Date)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Mos. Under Warranty" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Integer</returns>	
    <oDataColumn("Mos. Under Warranty")>
    Public Property WARRANTYMONTHS As Integer
        Get
            Return getProperty()
        End Get
        Set(value As Integer)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Price List Code" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Price List Code", length:=6)>
    Public Property PLNAME As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Details" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Details", length:=48)>
    Public Property DETAILS As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Update Versions Only" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Update Versions Only", length:=1)>
    Public Property VERFLAG As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get the Read Only "Current Version" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Integer</returns>	
    <oDataColumn("Current Version", ReadOnly:=True)>
    Public Readonly Property CURVERSION As Integer
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "MS-Project File" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("MS-Project File", ReadOnly:=True, length:=66)>
    Public Readonly Property EXTFILENAME As String
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get / Set the "No. of Users at Cust" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Integer</returns>	
    <oDataColumn("No. of Users at Cust")>
    Public Property NUSERS As Integer
        Get
            Return getProperty()
        End Get
        Set(value As Integer)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Reference" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Reference", length:=16)>
    Public Property BOOKNUM As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Miles to Charge" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Integer</returns>	
    <oDataColumn("Miles to Charge")>
    Public Property KM As Integer
        Get
            Return getProperty()
        End Get
        Set(value As Integer)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Travel Tm to Charge" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Travel Tm to Charge", length:=6)>
    Public Property ROADTIME As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Miles-Internal Rept" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Integer</returns>	
    <oDataColumn("Miles-Internal Rept")>
    Public Property KMI As Integer
        Get
            Return getProperty()
        End Get
        Set(value As Integer)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Travel Tm (Internal)" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Travel Tm (Internal)", length:=6)>
    Public Property ROADTIMEI As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get the Read Only "Total for Proj Items" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("Total for Proj Items", ReadOnly:=True)>
    Public Readonly Property QPRICE As Decimal
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get / Set the "% Overall Discount" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("% Overall Discount")>
    Public Property PERCENT As Decimal
        Get
            Return getProperty()
        End Get
        Set(value As Decimal)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get the Read Only "Price After Discount" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("Price After Discount", ReadOnly:=True)>
    Public Readonly Property DISPRICE As Decimal
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Sales Tax" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("Sales Tax", ReadOnly:=True)>
    Public Readonly Property VAT As Decimal
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get / Set the "Tax Code" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Tax Code", mandatory:=True, length:=8)>
    Public Property TAXCODE As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get the Read Only "Total Project Price" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("Total Project Price", ReadOnly:=True)>
    Public Readonly Property TOTPRICE As Decimal
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Profit-FinalProducts" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("Profit-FinalProducts", ReadOnly:=True)>
    Public Readonly Property PROFIT As Decimal
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Cost Calc. Date" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Date</returns>	
    <oDataColumn("Cost Calc. Date", ReadOnly:=True)>
    Public Readonly Property CALCDATE As Date
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get / Set the "Curr" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Curr", mandatory:=True, length:=3)>
    Public Property CODE As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get the Read Only "Service Price" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("Service Price", ReadOnly:=True)>
    Public Readonly Property SERVPRICE As Decimal
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Total Planned Price" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("Total Planned Price", ReadOnly:=True)>
    Public Readonly Property ACCQPRICE As Decimal
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Cumul. Material Cost" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("Cumul. Material Cost", ReadOnly:=True)>
    Public Readonly Property ACCMATERIALCOST As Decimal
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Date of Last Change" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Date</returns>	
    <oDataColumn("Date of Last Change", ReadOnly:=True)>
    Public Readonly Property TDATE As Date
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Cumul. Labor Cost" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("Cumul. Labor Cost", ReadOnly:=True)>
    Public Readonly Property ACCLABORCOST As Decimal
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Total Planned Cost" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("Total Planned Cost", ReadOnly:=True)>
    Public Readonly Property ACCCOST As Decimal
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Total Planned Profit" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("Total Planned Profit", ReadOnly:=True)>
    Public Readonly Property ACCPROFIT As Decimal
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Curr" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Curr", ReadOnly:=True, length:=3)>
    Public Readonly Property CODEB As String
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Cumul. Planned Hrs" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Decimal</returns>	
    <oDataColumn("Cumul. Planned Hrs", ReadOnly:=True)>
    Public Readonly Property ACCPLANHOURS As Decimal
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get / Set the "Purch for Project" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Purch for Project", length:=1)>
    Public Property PURFLAG As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Contact" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Contact", length:=37)>
    Public Property NAME As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Telephone" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Telephone", length:=20)>
    Public Property PHONE As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get the Read Only "Attachments?" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Attachments?", ReadOnly:=True, length:=1)>
    Public Readonly Property EXTFILEFLAGB As String
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get / Set the "Exclude from Receipt" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Exclude from Receipt", length:=1)>
    Public Property EDIFLAG As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "No Reporting?" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("No Reporting?", length:=1)>
    Public Property NOTFORREPORT As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Non-billable Hours" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Non-billable Hours", length:=1)>
    Public Property NOTBILLFLAG As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Authorize Hours?" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Authorize Hours?", length:=1)>
    Public Property APPFLAG As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get the Read Only "Signature" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Signature", ReadOnly:=True, length:=20)>
    Public Readonly Property USERLOGIN As String
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get the Read Only "Time Stamp" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Date</returns>	
    <oDataColumn("Time Stamp", ReadOnly:=True)>
    Public Readonly Property UDATE As Date
        Get
            Return getProperty()
        End Get

    End Property
    
	''' <summary>
    ''' Get / Set the "Document (ID)" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>Integer</returns>	
    <oDataColumn("Document (ID)")>
    Public Property DOCA As Integer
        Get
            Return getProperty()
        End Get
        Set(value As Integer)
            setProperty(value)
        End Set
    End Property
    
	''' <summary>
    ''' Get / Set the "Type" Value of DOCUMENTS_p.
    ''' </summary>
    ''' <returns>String</returns>	
    <oDataColumn("Type", Key:=True, length:=1)>
    Public Property TYPE As String
        Get
            Return getProperty()
        End Get
        Set(value As String)
            setProperty(value)
        End Set
    End Property


End Class

''' <summary>
''' A nullable version of rowDOCUMENTS_p.
'''
''' This is used to deserialise JSON data
''' and should not be instantiated directly.
''' </summary>
Public Class serialDOCUMENTS_p 

    Public Property CUSTNAME As String
    Public Property CUSTDES As String
    Public Property DOCNO As String
    Public Property PROJDES As String
    Public Property EPROJDES As String
    Public Property MUSERLOGIN As String
    Public Property SUSERLOGIN As String
    Public Property FLAG As String
    Public Property STATDES As String
    Public Property PROJTYPECODE As String
    Public Property PROJTYPEDES As String
    Public Property PROJTYPECODE2 As String
    Public Property PROJTYPEDES2 As String
    Public Property CTYPECODE As String
    Public Property CTYPENAME As String
    Public Property CURDATE As Date?
    Public Property AUTOVERCALC As String
    Public Property INSTDATE As Date?
    Public Property WARRANTYMONTHS As Integer?
    Public Property PLNAME As String
    Public Property DETAILS As String
    Public Property VERFLAG As String
    Public Property CURVERSION As Integer?
    Public Property EXTFILENAME As String
    Public Property NUSERS As Integer?
    Public Property BOOKNUM As String
    Public Property KM As Integer?
    Public Property ROADTIME As String
    Public Property KMI As Integer?
    Public Property ROADTIMEI As String
    Public Property QPRICE As Decimal?
    Public Property PERCENT As Decimal?
    Public Property DISPRICE As Decimal?
    Public Property VAT As Decimal?
    Public Property TAXCODE As String
    Public Property TOTPRICE As Decimal?
    Public Property PROFIT As Decimal?
    Public Property CALCDATE As Date?
    Public Property CODE As String
    Public Property SERVPRICE As Decimal?
    Public Property ACCQPRICE As Decimal?
    Public Property ACCMATERIALCOST As Decimal?
    Public Property TDATE As Date?
    Public Property ACCLABORCOST As Decimal?
    Public Property ACCCOST As Decimal?
    Public Property ACCPROFIT As Decimal?
    Public Property CODEB As String
    Public Property ACCPLANHOURS As Decimal?
    Public Property PURFLAG As String
    Public Property NAME As String
    Public Property PHONE As String
    Public Property EXTFILEFLAGB As String
    Public Property EDIFLAG As String
    Public Property NOTFORREPORT As String
    Public Property NOTBILLFLAG As String
    Public Property APPFLAG As String
    Public Property USERLOGIN As String
    Public Property UDATE As Date?
    Public Property DOCA As Integer?
    Public Property TYPE As String


End Class