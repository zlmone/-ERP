Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Data.Sql
Namespace LFERP.Library.SampleManager.SampleSend
    Public Class SampleSendControler
        Public Function SampleSend_Add(ByVal objinfo As SampleSendInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSend_Add")

                db.AddInParameter(dbComm, "SP_ID", DbType.String, objinfo.SP_ID)
                db.AddInParameter(dbComm, "SO_ID", DbType.String, objinfo.SO_ID)
                db.AddInParameter(dbComm, "SS_Edition", DbType.String, objinfo.SS_Edition)
                db.AddInParameter(dbComm, "SP_Qty", DbType.Int32, objinfo.SP_Qty)
                db.AddInParameter(dbComm, "SP_CusterID", DbType.String, objinfo.SP_CusterID)
                db.AddInParameter(dbComm, "SP_SendDate", DbType.DateTime, CDate(objinfo.SP_SendDate))
                db.AddInParameter(dbComm, "CO_ID", DbType.String, objinfo.CO_ID)
                db.AddInParameter(dbComm, "SP_AddUserID", DbType.String, objinfo.SP_AddUserID)
                db.AddInParameter(dbComm, "SP_AddDate", DbType.DateTime, CDate(objinfo.SP_AddDate))
                db.AddInParameter(dbComm, "SP_Remark", DbType.String, objinfo.SP_Remark)
                'db.AddInParameter(dbComm, "SP_ModifyUserID", DbType.String, objinfo.SP_ModifyUserID)
                'db.AddInParameter(dbComm, "SP_ModifyDate", DbType.DateTime, objinfo.SP_ModifyDate)
                db.AddInParameter(dbComm, "PM_M_Code", DbType.String, objinfo.PM_M_Code)
                db.AddInParameter(dbComm, "M_Code", DbType.String, objinfo.M_Code)
                db.AddInParameter(dbComm, "SP_ExpCompany", DbType.String, objinfo.SP_ExpCompany)
                db.AddInParameter(dbComm, "SP_ExpDeliveryID", DbType.String, objinfo.SP_ExpDeliveryID)
                db.AddInParameter(dbComm, "PK_Code_ID", DbType.String, objinfo.PK_Code_ID)

                db.AddInParameter(dbComm, "@SP_LC", DbType.String, objinfo.SP_LC)
                db.AddInParameter(dbComm, "@SP_BigBox", DbType.String, objinfo.SP_BigBox)
                db.AddInParameter(dbComm, "@SP_CardID", DbType.String, objinfo.SP_CardID)

                '2014-05-22 袁毅龙
                db.AddInParameter(dbComm, "@SenBoxSupplyNum", DbType.String, objinfo.SenBoxSupplyNum)
                '------------------------------------

                db.ExecuteNonQuery(dbComm)
                SampleSend_Add = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleSend_Add = False
            End Try
        End Function

        Public Function SampleSend_Update(ByVal objinfo As SampleSendInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSend_Update")

                db.AddInParameter(dbComm, "SP_ID", DbType.String, objinfo.SP_ID)
                db.AddInParameter(dbComm, "SO_ID", DbType.String, objinfo.SO_ID)
                db.AddInParameter(dbComm, "SS_Edition", DbType.String, objinfo.SS_Edition)
                db.AddInParameter(dbComm, "SP_Qty", DbType.Int32, objinfo.SP_Qty)
                db.AddInParameter(dbComm, "SP_CusterID", DbType.String, objinfo.SP_CusterID)
                db.AddInParameter(dbComm, "SP_SendDate", DbType.DateTime, CDate(objinfo.SP_SendDate))
                db.AddInParameter(dbComm, "CO_ID", DbType.String, objinfo.CO_ID)
                db.AddInParameter(dbComm, "SP_ModifyUserID", DbType.String, objinfo.SP_ModifyUserID)
                db.AddInParameter(dbComm, "SP_ModifyDate", DbType.DateTime, objinfo.SP_ModifyDate)
                db.AddInParameter(dbComm, "PM_M_Code", DbType.String, objinfo.PM_M_Code)
                db.AddInParameter(dbComm, "M_Code", DbType.String, objinfo.M_Code)
                db.AddInParameter(dbComm, "AutoID", DbType.String, objinfo.AutoID)
                db.AddInParameter(dbComm, "SP_Remark", DbType.String, objinfo.SP_Remark)
                db.AddInParameter(dbComm, "SP_ExpCompany", DbType.String, objinfo.SP_ExpCompany)
                db.AddInParameter(dbComm, "SP_ExpDeliveryID", DbType.String, objinfo.SP_ExpDeliveryID)
                db.AddInParameter(dbComm, "PK_Code_ID", DbType.String, objinfo.PK_Code_ID)

                db.AddInParameter(dbComm, "@SP_LC", DbType.String, objinfo.SP_LC)
                db.AddInParameter(dbComm, "@SP_BigBox", DbType.String, objinfo.SP_BigBox)
                db.AddInParameter(dbComm, "@SP_CardID", DbType.String, objinfo.SP_CardID)

                db.ExecuteNonQuery(dbComm)
                SampleSend_Update = True

            Catch ex As Exception
                MsgBox(ex.Message)
                SampleSend_Update = False
            End Try
        End Function


        Public Function SampleSend_Delete(ByVal SP_ID As String) As Boolean

            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSend_Delete")
                db.AddInParameter(dbComm, "@SP_ID", DbType.String, SP_ID)
                db.ExecuteNonQuery(dbComm)
                SampleSend_Delete = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleSend_Delete = False
            End Try
        End Function

        Public Function SampleSend_DeleteAutoID(ByVal AutoID As String) As Boolean

            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSend_DeleteAutoID")
                db.AddInParameter(dbComm, "@AutoID", DbType.Decimal, AutoID)
                db.ExecuteNonQuery(dbComm)
                SampleSend_DeleteAutoID = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleSend_DeleteAutoID = False
            End Try
        End Function

        Public Function SampleSend_UpdateNoSendQty(ByVal objinfo As SampleSendInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSend_UpdateNoSendQty")

                db.AddInParameter(dbComm, "@SO_ID", DbType.String, objinfo.SO_ID)
                db.AddInParameter(dbComm, "@SS_Edition", DbType.String, objinfo.SS_Edition)
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, objinfo.PM_M_Code)
                db.AddInParameter(dbComm, "@SC_ConfirmationQty", DbType.Int32, objinfo.SP_Qty)
                db.ExecuteNonQuery(dbComm)
                SampleSend_UpdateNoSendQty = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleSend_UpdateNoSendQty = False
            End Try
        End Function

        Public Function SampleSend_UpdateInCheck(ByVal objinfo As SampleSendInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSend_UpdateInCheck")

                db.AddInParameter(dbComm, "SP_ID", DbType.String, objinfo.SP_ID)
                db.AddInParameter(dbComm, "SP_InCheck", DbType.Boolean, objinfo.SP_InCheck)
                db.AddInParameter(dbComm, "SP_InCheckDate", DbType.Date, objinfo.SP_InCheckDate)
                db.AddInParameter(dbComm, "SP_InCheckUserID", DbType.String, objinfo.SP_InCheckUserID)
                db.AddInParameter(dbComm, "SP_InCheckRemark", DbType.String, objinfo.SP_InCheckRemark)
                db.ExecuteNonQuery(dbComm)
                SampleSend_UpdateInCheck = True

            Catch ex As Exception
                MsgBox(ex.Message)
                SampleSend_UpdateInCheck = False
            End Try
        End Function

        Public Function SampleSend_UpdateCheck(ByVal objinfo As SampleSendInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSend_UpdateCheck")

                db.AddInParameter(dbComm, "SP_ID", DbType.String, objinfo.SP_ID)
                db.AddInParameter(dbComm, "SP_Check", DbType.Boolean, objinfo.SP_Check)
                db.AddInParameter(dbComm, "SP_CheckDate", DbType.Date, objinfo.SP_CheckDate)
                db.AddInParameter(dbComm, "SP_CheckUserID", DbType.String, objinfo.SP_CheckUserID)
                db.AddInParameter(dbComm, "SP_CheckRemark", DbType.String, objinfo.SP_CheckRemark)
                db.ExecuteNonQuery(dbComm)
                SampleSend_UpdateCheck = True

            Catch ex As Exception
                MsgBox(ex.Message)
                SampleSend_UpdateCheck = False
            End Try
        End Function


        Public Function SampleSend_Getlist(ByVal SP_ID As String, ByVal SO_ID As String, ByVal SS_Edition As String, ByVal M_Code As String, ByVal SP_CusterID As String, ByVal PM_M_Code As String, ByVal SP_Check As String, ByVal StartDate As String, ByVal EndDate As String, ByVal SP_AddStartDate As String, ByVal SP_AddEndDate As String, ByVal SO_IDCheck As Boolean, ByVal SP_AddUserID As String) As List(Of SampleSendInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSend_Getlist")

            db.AddInParameter(dbComm, "@SP_ID", DbType.String, SP_ID)
            db.AddInParameter(dbComm, "@SO_ID", DbType.String, SO_ID)
            db.AddInParameter(dbComm, "@SS_Edition", DbType.String, SS_Edition)
            db.AddInParameter(dbComm, "@M_Code", DbType.String, M_Code)
            db.AddInParameter(dbComm, "@SP_CusterID", DbType.String, SP_CusterID)
            db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)
            db.AddInParameter(dbComm, "@SP_Check", DbType.Boolean, SP_Check)
            db.AddInParameter(dbComm, "@StartDate", DbType.String, StartDate)
            db.AddInParameter(dbComm, "@EndDate", DbType.String, EndDate)
            db.AddInParameter(dbComm, "@SP_AddStartDate", DbType.String, SP_AddStartDate)
            db.AddInParameter(dbComm, "@SP_AddEndDate", DbType.String, SP_AddEndDate)
            db.AddInParameter(dbComm, "@SO_IDCheck", DbType.Boolean, SO_IDCheck)
            db.AddInParameter(dbComm, "@SP_AddUserID", DbType.String, SP_AddUserID)


            Dim FeatureList As New List(Of SampleSendInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleSendType(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Public Function SampleSendSP_GetList(ByVal SP_ID As String, ByVal AutoID As String, ByVal Type As String) As List(Of SampleSendInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSendSP_GetList")

            db.AddInParameter(dbComm, "@SP_ID", DbType.String, SP_ID)
            db.AddInParameter(dbComm, "@AutoID", DbType.String, AutoID)
            db.AddInParameter(dbComm, "@Type", DbType.String, Type)
            Dim FeatureList As New List(Of SampleSendInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleSendType(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Public Function SampleSend_Get(ByVal SP_ID As String) As SampleSendInfo
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSend_Get")
            db.AddInParameter(dbComm, "@SP_ID", DbType.String, SP_ID)
            Dim FeatureList As New SampleSendInfo
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.SP_ID = reader("SP_ID").ToString
                End While
                Return FeatureList
            End Using
        End Function

        Public Function SampleSend_GetQty(ByVal SO_ID As String, ByVal SS_Edition As String) As Integer
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSend_GetQty")

            db.AddInParameter(dbComm, "@SO_ID", DbType.String, SO_ID)
            db.AddInParameter(dbComm, "@SS_Edition", DbType.String, SS_Edition)

            Dim StrCode_ID As Integer = 0
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    StrCode_ID = CInt(reader("Qty").ToString)
                End While
                SampleSend_GetQty = StrCode_ID
            End Using
        End Function


        ''' <summary>
        ''' 2014-04-29
        ''' 獲取FTP參數設置
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Function FillSampleSendFTPSet(ByVal reader As IDataReader) As SampleSendInfo
            On Error Resume Next
            Dim objInfo As New SampleSendInfo

            objInfo.FTPServer = reader("FTPServer").ToString
            objInfo.IPAddress = reader("IPAddress").ToString
            objInfo.FTPuser = reader("FTPuser").ToString
            objInfo.FTPPassWord = reader("FTPPassWord").ToString
            objInfo.FTPRemoteDir = reader("FTPRemoteDir").ToString
            objInfo.FTPPort = reader("FTPPort").ToString

            Return objInfo
        End Function

        Friend Function FillSampleSendType(ByVal reader As IDataReader) As SampleSendInfo
            '对应取得的数据
            On Error Resume Next
            Dim objInfo As New SampleSendInfo

            objInfo.SP_ID = reader("SP_ID").ToString
            objInfo.SO_ID = reader("SO_ID").ToString
            objInfo.SS_Edition = reader("SS_Edition").ToString
            objInfo.PS_NO = reader("PS_NO").ToString
            objInfo.SP_Qty = CInt(reader("SP_Qty").ToString)
            objInfo.SP_SendDate = CDate(reader("SP_SendDate").ToString)
            objInfo.SP_CusterID = reader("SP_CusterID").ToString
            objInfo.PS_NO = reader("PS_NO").ToString
            objInfo.PM_M_Code = reader("PM_M_Code").ToString
            objInfo.M_Code = reader("M_Code").ToString
            objInfo.M_Name = reader("M_Name").ToString
            objInfo.PM_M_Name = reader("PM_M_Name").ToString
            objInfo.SP_AddUserName = reader("SP_AddUserName").ToString
            objInfo.SP_AddDate = CDate(reader("SP_AddDate").ToString)
            objInfo.SP_AddUserID = reader("SP_AddUserID").ToString
            objInfo.SP_ModifyUserID = reader("SP_ModifyUserID").ToString
            objInfo.SP_ModifyDate = CDate(reader("SP_ModifyDate").ToString)

            objInfo.SP_Check = CBool(reader("SP_Check").ToString)
            objInfo.SP_CheckDate = CDate(reader("SP_CheckDate").ToString)
            objInfo.SP_CheckRemark = reader("SP_CheckRemark").ToString
            objInfo.SP_CheckUserID = reader("SP_CheckUserID").ToString
            objInfo.SP_CheckUserName = reader("SP_CheckUserName").ToString

            objInfo.SP_InCheck = CBool(reader("SP_InCheck").ToString)
            objInfo.SP_InCheckDate = CDate(reader("SP_InCheckDate").ToString)
            objInfo.SP_InCheckRemark = reader("SP_InCheckRemark").ToString
            objInfo.SP_InCheckUserID = reader("SP_InCheckUserID").ToString
            objInfo.SP_InCheckUserName = reader("SP_InCheckUserName").ToString

            objInfo.SP_Remark = reader("SP_Remark").ToString
            objInfo.C_ChsName = reader("C_ChsName").ToString
            objInfo.AutoID = CDbl(reader("AutoID").ToString)
            objInfo.SP_ExpDeliveryID = reader("SP_ExpDeliveryID").ToString
            objInfo.SP_ExpCompany = reader("SP_ExpCompany").ToString
            objInfo.SO_SampleID = reader("SO_SampleID").ToString
            objInfo.PK_Code_ID = reader("PK_Code_ID").ToString

            '2014-05-13  姚駿
            objInfo.CVSName = reader("CVSName").ToString
            objInfo.CreateDate = CDate(reader("CreateDate")).ToString("yyyy/MM/dd")
            objInfo.UploadDate = CDate(reader("UploadDate")).ToString("yyyy/MM/dd")
            objInfo.Flag = reader("Flag").ToString
            objInfo.SP_LC = reader("SP_LC").ToString
            objInfo.SP_BigBox = reader("SP_BigBox").ToString
            objInfo.FTPFileName = reader("FTPFileName").ToString
            objInfo.SP_CardID = reader("SP_CardID").ToString
            objInfo.SO_CusterNo = reader("SO_CusterNo").ToString

            '2014-05-11 袁毅龙
            objInfo.SenBoxSupplyNum = reader("SenBoxSupplyNum").ToString
            Return objInfo
        End Function

#Region "獲取單號"
        ''' <summary>
        ''' FTP上传名称
        ''' </summary>
        ''' <param name="CVSName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SampleSendFtp_GetCVsName(ByVal CVSName As String) As SampleSendInfo

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSendFtp_GetCVsName")

            db.AddInParameter(dbComm, "@CVSName", DbType.String, CVSName)

            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    Return FillSampleSendType(reader)
                End While
                Return Nothing
            End Using
        End Function
#End Region

#Region "添加FTP数据"
        ''' <summary>
        ''' 2014-04-29
        ''' 添加FTP数据
        ''' </summary>
        ''' <param name="objinfo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SampleSendFtp_AddCVsName(ByVal objinfo As SampleSendInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSendFtp_AddCVsName")

                db.AddInParameter(dbComm, "@SP_ID", DbType.String, objinfo.SP_ID)
                db.AddInParameter(dbComm, "@CVSName", DbType.String, objinfo.CVSName)
                db.AddInParameter(dbComm, "@CreateDate", DbType.String, objinfo.CreateDate)
                db.AddInParameter(dbComm, "@OperationUserID", DbType.String, objinfo.OperationUserID)


                db.ExecuteNonQuery(dbComm)
                SampleSendFtp_AddCVsName = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleSendFtp_AddCVsName = False
            End Try
        End Function
#End Region

#Region "更新FTP数据"
        ''' <summary>
        ''' 2014-04-29
        ''' 更新FTP数据
        ''' </summary>
        ''' <param name="objinfo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SampleSendFtp_UpdateCVsName(ByVal objinfo As SampleSendInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSendFtp_UpdateCVsName")

                db.AddInParameter(dbComm, "@CVSName", DbType.String, objinfo.CVSName)
                db.AddInParameter(dbComm, "@UploadDate", DbType.String, objinfo.UploadDate)
                db.AddInParameter(dbComm, "@Flag", DbType.String, objinfo.Flag)

                db.ExecuteNonQuery(dbComm)
                SampleSendFtp_UpdateCVsName = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleSendFtp_UpdateCVsName = False
            End Try
        End Function
#End Region

#Region "FTP參數設置"
        ''' <summary>
        '''  2014-04-29
        ''' FTP參數設置
        ''' </summary>
        ''' <param name="objinfo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SampleSendSetFtp_Add(ByVal objinfo As SampleSendInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSendSetFtp_Add")

                db.AddInParameter(dbComm, "@FTPServer", SqlDbType.NVarChar, objinfo.FTPServer)
                db.AddInParameter(dbComm, "@IPAddress", DbType.String, objinfo.IPAddress)
                db.AddInParameter(dbComm, "@FTPuser", DbType.String, objinfo.FTPuser)
                db.AddInParameter(dbComm, "@FTPPassWord", DbType.String, objinfo.FTPPassWord)
                db.AddInParameter(dbComm, "@FTPRemoteDir", DbType.String, objinfo.FTPRemoteDir)
                db.AddInParameter(dbComm, "@FTPPort", DbType.String, objinfo.FTPPort)

                db.ExecuteNonQuery(dbComm)
                SampleSendSetFtp_Add = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleSendSetFtp_Add = False
            End Try
        End Function
#End Region

#Region "寄送單號判斷"
        ''' <summary>
        ''' 寄送單號判斷
        ''' 2014-04-29
        ''' 姚      駿
        ''' </summary>
        ''' <param name="Code_ID"></param>
        ''' <param name="Flag"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SampleSend_GetListOne(ByVal Code_ID As String, ByVal Flag As String) As List(Of SampleSendInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSend_GetListOne")

            db.AddInParameter(dbComm, "@Code_ID", DbType.String, Code_ID)
            db.AddInParameter(dbComm, "@Flag", DbType.String, Flag)

            Dim FeatureList As New List(Of SampleSendInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleSendType(reader))
                End While
                Return FeatureList
            End Using
        End Function
#End Region

#Region "獲取FTP參數"
        ''' <summary>
        '''  2014-04-29
        ''' 獲取FTP參數
        ''' 姚      駿
        ''' </summary>
        ''' <param name="FTPServer"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SampleSendSetFtp_GetList(ByVal FTPServer As String) As List(Of SampleSendInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSendSetFtp_GetList")

            db.AddInParameter(dbComm, "@FTPServer", SqlDbType.NVarChar, FTPServer)

            Dim FeatureList As New List(Of SampleSendInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleSendFTPSet(reader))
                End While
                Return FeatureList
            End Using
        End Function
        ''' <summary>
        ''' 2014-04-29
        ''' 獲取FTP參數
        ''' 姚      駿
        ''' </summary>
        ''' <param name="FTPServer"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SampleSendSetFtp_GetListSQL(ByVal FTPServer As String) As List(Of SampleSendInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)

            Dim FeatureList As New List(Of SampleSendInfo)
            Using reader As IDataReader = db.ExecuteReader(CommandType.Text, FTPServer)
                While reader.Read
                    FeatureList.Add(FillSampleSendFTPSet(reader))
                End While
                Return FeatureList
            End Using
        End Function

        ''' <summary>
        ''' 修改FTP文件名
        ''' 2014-05-26
        ''' 姚      駿
        ''' </summary>
        ''' <param name="objinfo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SampleSend_UpdateFTPFileName(ByVal objinfo As SampleSendInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSend_UpdateFTPFileName")

                db.AddInParameter(dbComm, "@SP_ID", DbType.String, objinfo.SP_ID)
                db.AddInParameter(dbComm, "@FTPFileName", DbType.String, objinfo.FTPFileName)
                db.ExecuteNonQuery(dbComm)
                SampleSend_UpdateFTPFileName = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleSend_UpdateFTPFileName = False
            End Try
        End Function

        ''' <summary>
        ''' 獲取CSV數據
        ''' 2014-05-22 
        ''' 姚      駿
        ''' </summary>
        ''' <param name="SP_ID"></param>
        ''' <param name="PM_M_Code"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SampleSendShipFiles_CSVGetList(ByVal SP_ID As String, ByVal PM_M_Code As String) As DataTable
            Try
                Dim ds As New DataSet
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSendShipFiles_CSVGetList")
                db.AddInParameter(dbComm, "@SP_ID", DbType.String, SP_ID)
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)

                ds = db.ExecuteDataSet(dbComm)
                If ds.Tables.Count > 0 Then
                    Return ds.Tables(0)
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
                Return Nothing
            End Try
        End Function
#End Region
#Region "袁毅龙 20140721 获取箱号 "
        Public Function SampleSendSupplyBoxNum_GetNO(ByVal SendBoxSupplyNum1 As String, ByVal SendBoxSupplyNum2 As String) As SampleSendInfo
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSendSupplyBoxNum_GetNO")
            db.AddInParameter(dbComm, "@SendBoxSupplyNum1", DbType.String, SendBoxSupplyNum1)
            db.AddInParameter(dbComm, "@SendBoxSupplyNum2", DbType.String, SendBoxSupplyNum2)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    Return FillSampleSendSupplyBoxNum_GetNO(reader)
                End While
                Return Nothing
            End Using
        End Function
        Friend Function FillSampleSendSupplyBoxNum_GetNO(ByVal reader As IDataReader) As SampleSendInfo
            On Error Resume Next
            Dim sampleSendInfo As New SampleSendInfo
            sampleSendInfo.SendBoxSupplyNum1 = reader("SendBoxSupplyNum1").ToString
            sampleSendInfo.SendBoxSupplyNum2 = reader("SendBoxSupplyNum2").ToString
            Return sampleSendInfo
        End Function
#End Region
#Region "'袁毅龙 20140721 记录新增的箱号"
        Public Function SampleSendSupplyBoxNum_Add(ByVal objinfo As SampleSendInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleSendSupplyBoxNum_Add")
                db.AddInParameter(dbComm, "@SendBoxSupplyNum1", DbType.String, objinfo.SendBoxSupplyNum1)
                db.AddInParameter(dbComm, "@SendBoxSupplyNum2", DbType.String, objinfo.SendBoxSupplyNum2)
                db.ExecuteNonQuery(dbComm)
                SampleSendSupplyBoxNum_Add = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleSendSupplyBoxNum_Add = False
            End Try
        End Function
#End Region
#Region "删除箱号"
        Public Function SampleSendSupplyBoxNum_DeleteNO(ByVal boxNum As String) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbcomm As DbCommand = db.GetStoredProcCommand("SampleSendSupplyBoxNum_DeleteNO")
                db.AddInParameter(dbcomm, "@SendBoxSupplyNum", DbType.String, boxNum)
                db.ExecuteNonQuery(dbcomm)
                SampleSendSupplyBoxNum_DeleteNO = True
            Catch ex As Exception
                SampleSendSupplyBoxNum_DeleteNO = False
                MsgBox(ex.Message)
            End Try
        End Function
#End Region
    End Class
End Namespace

