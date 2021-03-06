Namespace LFERP.Common.DBComExec
    Public Class DBComExec

#Region "緩存參數"
        Dim m_Con As New System.Data.SqlClient.SqlConnection(ConnStr)

        Public Property ConnectionString() As String
            Get
                Return m_Con.ConnectionString
            End Get
            Set(ByVal value As String)
                m_Con.ConnectionString = value
            End Set
        End Property

        Private Enum EXCUTETYPE As Integer                 '枚举描述
            ADD
            EDIT
            DELETE
            QUERY
        End Enum

#End Region

#Region "提取存儲過程所需參數"
        ''' <summary>
        ''' 2014-06-06
        ''' 姚      駿
        ''' </summary>
        ''' <param name="strProc">存儲過程名</param>
        ''' <returns>返回存儲過程參數列表</returns>
        ''' <remarks></remarks>
        Private Function GetProcParameters(ByVal strProc As String) As ArrayList
            Dim myComm As New System.Data.SqlClient.SqlCommand("sp_sproc_columns", m_Con)
            Dim dt As New DataTable

            myComm.CommandTimeout = 180      '默認兩分鐘

            Dim nIndex As Integer
            Dim obj As New Object
            obj = strProc
            myComm.CommandType = CommandType.StoredProcedure
            myComm.Parameters.AddWithValue("@procedure_name", obj)
            Dim myadapter As New System.Data.SqlClient.SqlDataAdapter(myComm)
            Try
                myadapter.Fill(dt)
            Catch ex As Exception

            Finally
                myadapter.Dispose()
            End Try

            Dim arrayListParam As New ArrayList()
            For nIndex = 1 To dt.Rows.Count - 1
                arrayListParam.Add(dt.Rows(nIndex)(3).ToString())
            Next
            Return arrayListParam
        End Function

        ''' <summary>
        ''' 查詢存儲過程的數據
        ''' 2014-06-06
        ''' 姚     駿
        ''' </summary>
        ''' <param name="strProc"></param>
        ''' <param name="parms"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDataProcedure(ByVal strProc As String, ByVal parms As Object()) As DataTable

            Dim myComm As New System.Data.SqlClient.SqlCommand(strProc, m_Con)
            myComm.CommandType = CommandType.StoredProcedure
            myComm.CommandTimeout = 180      '默認兩分鐘

            Dim arrayListParam As New ArrayList
            arrayListParam = GetProcParameters(strProc)

            Dim nIndex As Integer
            For nIndex = 0 To parms.Length - 1
                myComm.Parameters.AddWithValue(arrayListParam(nIndex).ToString(), parms(nIndex))
            Next
            Dim myAdapter As New SqlDataAdapter
            myAdapter.SelectCommand = myComm
            Dim dtTemp As New DataTable
            Try
                myAdapter.Fill(dtTemp)
            Catch ex As Exception

            Finally
                myAdapter.Dispose()
            End Try
            Return dtTemp
        End Function

#End Region

#Region "執行存儲過程的方法"
        ''' <summary>
        '''  執行存儲過程的方法
        ''' 如增刪改查
        ''' </summary>
        ''' <param name="strProc"></param>
        ''' <param name="parms"></param>
        ''' <param name="nType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExecuteDataProcedure(ByVal strProc As String, ByVal parms As Object(), ByVal nType As Integer) As Boolean

            Dim myComm As New System.Data.SqlClient.SqlCommand(strProc, m_Con)
            myComm.CommandType = CommandType.StoredProcedure
            myComm.CommandTimeout = 180      '默認兩分鐘

            Dim arrayListParam As New ArrayList
            arrayListParam = GetProcParameters(strProc)

            Dim nIndex As Integer
            For nIndex = 0 To parms.Length - 1
                myComm.Parameters.AddWithValue(arrayListParam(nIndex).ToString(), parms(nIndex))
            Next
            Dim myAdapter As New SqlDataAdapter

            Try
                m_Con.Open()
                Select Case nType
                    Case EXCUTETYPE.ADD
                        myAdapter.InsertCommand = myComm
                        myAdapter.InsertCommand.ExecuteNonQuery()
                    Case EXCUTETYPE.EDIT
                        myAdapter.UpdateCommand = myComm
                        myAdapter.UpdateCommand.ExecuteNonQuery()
                    Case EXCUTETYPE.DELETE
                        myAdapter.DeleteCommand = myComm
                        myAdapter.DeleteCommand.ExecuteNonQuery()
                End Select

                ExecuteDataProcedure = True
            Catch ex As Exception
                ExecuteDataProcedure = False
            Finally
                myAdapter.Dispose()
                m_Con.Close()
            End Try

        End Function
#End Region

#Region "執行SQL語句"
        ''' <summary>
        ''' SQL執行命令
        ''' </summary>
        ''' <param name="strSQL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExecuteDataSQL(ByVal strSQL As String) As DataTable
            Dim myComm As New System.Data.SqlClient.SqlCommand(strSQL, m_Con)
            myComm.CommandType = CommandType.Text
            myComm.CommandTimeout = 180      '默認兩分鐘

            Dim myAdapter As New SqlDataAdapter
            myAdapter.SelectCommand = myComm
            Dim dtTemp As New DataTable
            Try
                myAdapter.Fill(dtTemp)
            Catch ex As Exception
                dtTemp = Nothing
            Finally
                myAdapter.Dispose()
            End Try
            Return dtTemp
        End Function
#End Region


    End Class

End Namespace
