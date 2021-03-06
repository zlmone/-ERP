Imports LFERP.DataSetting
Imports LFERP.Library.SampleManager.SampleOrdersMain
Public Class frmSampleOrdersType
    Dim socon As New SampleOrdersMainControler
    Private _SO_ID As String
    Private _EditItem As String

    Property EditItem() As String '属性
        Get
            Return _EditItem
        End Get
        Set(ByVal value As String)
            _EditItem = value
        End Set
    End Property

    Property SO_ID() As String '属性
        Get
            Return _SO_ID
        End Get
        Set(ByVal value As String)
            _SO_ID = value
        End Set
    End Property

    Private Sub frmSampleOrdersType_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim solist As New List(Of SampleOrdersMainInfo)
        solist = socon.SampleOrdersMain_GetList(SO_ID, Nothing, Nothing, Nothing, Nothing, Nothing, True)
        If solist.Count > 0 Then
            Me.txtSO_ID.Text = SO_ID
            Me.txtPM_M_Code.Text = solist(0).PM_M_Code
            Me.txtSO_SampleID.Text = solist(0).SO_SampleID
            Me.txtM_Code_Type.Text = solist(0).M_Code_Type
            Me.cobTMaterialType.Text = solist(0).TMaterialType
            Me.cobOrdersType.EditValue = solist(0).SO_OrdersType
        End If

        If EditItem = EditEnumType.ADD Then
            Me.lblTitle.Text = Me.Text + EditEnumValue(EditEnumType.EDIT)
            Me.Text = Me.lblTitle.Text
        End If
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        If cobOrdersType.Text = String.Empty Then
            MsgBox("訂單类别不能为空！", MsgBoxStyle.Information, "溫馨提示")
            cobOrdersType.Focus()
            Exit Sub
        End If
        If txtM_Code_Type.Text = String.Empty Then
            MsgBox("产品类别不能为空！", MsgBoxStyle.Information, "溫馨提示")
            txtM_Code_Type.Focus()
            Exit Sub
        End If
        If cobTMaterialType.Text = String.Empty Then
            MsgBox("材料类别不能为空！", MsgBoxStyle.Information, "溫馨提示")
            cobTMaterialType.Focus()
            Exit Sub
        End If

        Dim soinfo As New SampleOrdersMainInfo
        soinfo.SO_ID = txtSO_ID.Text
        soinfo.M_Code_Type = txtM_Code_Type.Text
        soinfo.TMaterialType = cobTMaterialType.Text
        soinfo.SO_OrdersType = cobOrdersType.EditValue

        If socon.SampleOrdersMain_UpdateType(soinfo) = False Then
            MsgBox("更改产品类别错误！", MsgBoxStyle.Information, "溫馨提示")
            Exit Sub
        End If
        MsgBox("更改产品类别成功！", MsgBoxStyle.Information, "溫馨提示")
        Me.Close()
    End Sub
End Class