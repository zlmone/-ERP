Imports System.Collections.Specialized


Public Class Calculator
    ' ' '////////////////////////////////////////////////////////////////////////////////// 
    ' ' '//使用方法：   
    ' ' '//1、不含变量： 
    ' ' '//dim   expression   as   string   =   "1+32*9+Round(12*(1+9)) " 
    ' ' '//dim   results()   as     decimal=   Calculator.Eval(   expression,Nothing) 
    ' ' '//results(0)就是返回值 
    ' ' '//2、含变量： 
    ' ' '//dim   expression   as   string   =   "a+23*b " 
    ' ' '//dim   parameters   as   NameValueCollection=   new   NameValueCollection() 
    ' ' '//parameters.Add( "a ", "12.234 ") 
    ' ' '//parameters.Add( "b ", "34 ") 
    ' ' '//dim   results()   as     decimal   =   Calculator.Eval(   expression,parameters) 
    ' ' '//results(0)就是返回值 
    ' ' '//////////////////////////////////////////////////////////////////////////////////// 


    Protected Const MAX_LEVEL As Integer = 99               ' ' '最高运算级别常量 
    Private _data As NameValueCollection                         ' ' '变量参数 
    Private _opt As String                                                     ' ' '运算符 
    Private _expression As String                                       ' ' '表达式 
    Private _leftValue As String                                         ' ' '运算符左边的值 
    Private _rightValue As String                                       ' ' '运算符右边的值 

    Public Shared Function Eval(ByVal expression As String, ByVal dataProvider As NameValueCollection) As Decimal()
        Dim mcalc As Calculator = New Calculator(expression, dataProvider)
        Dim r As Decimal() = mcalc.Calculate()
        Return r
    End Function

    ' ' '构造函数 
    Public Sub New(ByVal expression As String, ByVal dataProvider As NameValueCollection)

        _expression = expression
        _data = dataProvider
        _rightValue = expression.ToUpper()
        If GetIndex(_rightValue) <> -1 Then
            strMsg = "1"
            MsgBox("表达式 " & _expression & "缺少 (") 'Throw New Exception("表达式 " & _expression & "缺少 (")
            Exit Sub
        End If
        Call Initialize()
    End Sub

    ' ' '初始化对象(将表达式拆分为左边的值、运算符和右边的表达式) 
    Private Sub Initialize()
        Dim right As String = ""

        GetNext(_rightValue, _leftValue, _opt, right)
        _rightValue = right
    End Sub

    ' ' '   获取运算符的级别 
    Private Shared Function GetOperatorLevel(ByVal strOperator As String) As Integer
        Dim i As Integer
        For i = 0 To Level.GetLength(0) - 1
            If CType(Level(i, 0), String) = strOperator Then Return CType(Level(i, 1), Integer)
        Next
        Return -1
    End Function

    ' ' '   如果字符串是以运算符开头,则返回该运算符,否则返回Nothing 
    Private Shared Function GetOperator(ByVal str As String) As String
        Dim i As Integer
        For i = 0 To Level.GetLength(0) - 1
            If str.StartsWith(CType(Level(i, 0), String)) Then Return CType(Level(i, 0), String)
        Next
        Return Nothing
    End Function

#Region "运算符与支持的函数 "
    Private Shared Level(,) As Object = New Object(,) _
    { _
    {",", 0}, _
    {"=", 1}, _
    {">=", 1}, _
    {"<=", 1}, _
    {"<>", 1}, _
    {">", 1}, _
    {"<", 1}, _
    {"+", 2}, _
    {"-", 2}, _
    {"*", 3}, _
    {"/", 3}, _
    {"%", 3}, _
    {"NEG", 4}, _
    {"^", 5}, _
    {"(", MAX_LEVEL}, _
    {"ROUND(", MAX_LEVEL}, _
    {"ROUNDEX(", MAX_LEVEL}, _
    {"TRUNC(", MAX_LEVEL}, _
    {"MAX(", MAX_LEVEL}, _
    {"MIN(", MAX_LEVEL}, _
    {"ABS(", MAX_LEVEL}, _
    {"SUM(", MAX_LEVEL}, _
    {"AVERAGE(", MAX_LEVEL}, _
    {"SQRT(", MAX_LEVEL}, _
    {"EXP(", MAX_LEVEL}, _
    {"LOG(", MAX_LEVEL}, _
    {"LOG10(", MAX_LEVEL}, _
    {"SIN(", MAX_LEVEL}, _
    {"COS(", MAX_LEVEL}, _
    {"TAN(", MAX_LEVEL}, _
    {"IF(", MAX_LEVEL}, _
    {"NOT(", MAX_LEVEL}, _
    {"AND(", MAX_LEVEL}, _
    {"OR(", MAX_LEVEL} _
    }         ' ' '//数学函数//三角函数//条件函数    '這個為自定義的 ROUNDEX

#End Region



#Region "解析表达式 "

    ' ' '   拆分表达式 
    ' ' '///   <param   name= "expression "> 表达式 </param> 
    ' ' '///   <param   name= "left "> 左边的值 </param> 
    ' ' '///   <param   name= "opt "> 运算符 </param> 
    ' ' '///   <param   name= "right "> 右边的值 </param> 
    Private Sub GetNext(ByVal expression As String, ByRef left As String, ByRef opt As String, ByRef right As String)
        right = expression
        left = String.Empty
        opt = Nothing

        While right <> String.Empty
            opt = GetOperator(right)
            If opt <> Nothing Then
                right = right.Substring(opt.Length, right.Length - opt.Length)
                Exit While
            Else
                left = left & right.Chars(0)
                right = right.Substring(1, right.Length - 1)
            End If
        End While
        left = left.Trim()
        right = right.Trim()
    End Sub

    ' ' '计算表达式 
    ' ' '   <returns> 返回值(可能有多个,如逗号表达式会返回多个值) </returns> 
    Public Function Calculate() As Decimal()
        ' ' '//如果运算符为空，则直接返回左边的值 
        If _opt = Nothing Then
            Dim r As Decimal = Decimal.Zero
            Try
                r = Decimal.Parse(_leftValue)
            Catch
                Try
                    r = Decimal.Parse(_data(_leftValue))
                Catch
                    'Throw New Exception("表达式" & _expression & "中含有错误的格式: " & _leftValue)
                    MsgBox("表达式" & _expression & "中含有错误的格式: " & _leftValue)
                    strMsg = "2"
                End Try
            End Try
            Return New Decimal(0) {r}
        End If

        ' ' '//判断是否是最高优先级的运算符(括号和函数) 
        If GetOperatorLevel(_opt) <> MAX_LEVEL Then
            ' ' '//四则运算符中，只有当-左边无值的时候是单目运算 
            If _opt <> "- " And _leftValue = String.Empty Then strMsg = "3" : MsgBox("表达式  " & _expression & "中  " & _opt & "    运算符的左边需要值或表达式 ") 'Throw New Exception("表达式  " & _expression & "中  " & _opt & "    运算符的左边需要值或表达式 ")
            If _opt = "- " And _leftValue = String.Empty Then _opt = "NEG "
            If _rightValue = String.Empty Then strMsg = "4" : MsgBox("表达式  " & _expression & " 中  " & _opt & "    运算符的右边需要值或表达式 ") ' Throw New Exception("表达式  " & _expression & " 中  " & _opt & "    运算符的右边需要值或表达式 ")
            Return CalculateTwoParms()
        Else
            ' ' '//括号和函数左边都不需要值 
            If _leftValue <> String.Empty Then strMsg = "5" : MsgBox("表达式  " & _expression & " 中 " & _opt & "   运算符的左边不需要值或表达式 ") 'Throw New Exception("表达式  " & _expression & " 中 " & _opt & "   运算符的左边不需要值或表达式 ")
            Return CalculateFunction()
        End If
    End Function

    ' ' '计算函数(括号运算符被当作函数计算,所有的函数必须已右括号结尾) 
    Private Function CalculateFunction() As Decimal()

        On Error Resume Next
        ' ' '//查找对应的右括号 
        Dim inx As Integer = GetIndex(_rightValue)
        If inx = -1 Then strMsg = "6" : MsgBox("表达式" & _expression & " 缺少") : Exit Function 'Throw New Exception("表达式" & _expression & " 缺少")

        Dim l As String = _rightValue.Substring(0, inx)
        ' ' '//如果表达式已经完成，则返回计算结果，否则计算当前结果 
        ' ' '//并修改左值、运算符、右边表达式的值，然后调用Calculate继续运算 
        If inx = _rightValue.Length - 1 Then
            Return Calc(_opt, l)
        Else

            Dim left As String = "", right As String = "", op As String = ""
            _rightValue = _rightValue.Substring(inx + 1, _rightValue.Length - inx - 1)
            GetNext(_rightValue, left, op, right)
            Dim r As Decimal() = Calc(_opt, l)
            _leftValue = r(r.Length - 1).ToString()
            If op = Nothing Then strMsg = "7" : MsgBox("表达式  " & _expression & " 中 ) 运算符的右边需要运算符 ") 'Throw New Exception("表达式  " & _expression & " 中 ) 运算符的右边需要运算符 ")
            _opt = op
            _rightValue = right
            Return Calculate()
        End If
    End Function

    ' ' '获取第一个未封闭的右括号在字符串中的位置 
    ' ' '   <param   name= "expression "> 传入的字符串 </param> 
    Private Function GetIndex(ByVal expression As String) As Integer
        Dim count As Integer = 0
        Dim i As Integer
        For i = 0 To expression.Length - 1
            If expression.Chars(i) = ")" Then
                If count = 0 Then
                    Return i
                Else
                    count -= 1
                End If
            End If
            If expression.Chars(i) = "(" Then count += 1
        Next
        Return -1
    End Function

    ' ' '计算四则表达式 
    Private Function CalculateTwoParms() As Decimal()
        On Error Resume Next

        Dim left As String = "", right As String = "", op As String = ""
        GetNext(_rightValue, left, op, right)
        Dim result As Decimal(), r As Decimal()

        ' ' '//如果下一个运算符的级别不大于当前运算符，则计算当前的值 
        If op = Nothing Or (GetOperatorLevel(_opt) >= GetOperatorLevel(op)) Then
            r = Calc(_opt, _leftValue, left)
        Else
            ' ' '//如果下一个运算符的级别大于当前运算符 
            Dim ex As String = left
            ' ' '//则一直找到低于当前运算符级别的运算符，然后将该运算符和当前运算符中间的表达式 
            ' ' '//提取出来，新构造一个对象，运算中间级别高的表达式的值 
            ' ' '//然后将新对爱的结果当作右边的值于当前的左值以及运算符进行运算 
            While (GetOperatorLevel(_opt) < GetOperatorLevel(op) And right <> String.Empty)
                ex = ex & op
                If GetOperatorLevel(op) = MAX_LEVEL Then
                    Dim pos As Integer = GetIndex(right)
                    ex = ex & right.Substring(0, pos + 1)
                    right = right.Substring(pos + 1)
                End If
                GetNext(right, left, op, right)
                ex = ex & left
            End While
            Dim mcalc As Calculator = New Calculator(ex, _data)
            Dim rl As Decimal() = mcalc.Calculate()
            r = Calc(_opt, _leftValue, rl(rl.Length - 1).ToString)
        End If
        ' ' '//将上一步计算出来的结果作为当前的左值，然后将表达式剩下的部分作为当前的右边的表达式 
        ' ' '//然后将下一个运算符作为当前运算符，然后递归运算 
        _leftValue = r(r.Length - 1).ToString()
        _opt = op
        _rightValue = right

        Dim rr As Decimal() = Calculate()
        Dim i As Integer
        ReDim result(r.Length - 1 + rr.Length - 1)
        For i = 0 To r.Length - 2
            result(i) = r(i)
        Next
        For i = 0 To rr.Length - 1
            result(r.Length - 1 + i) = rr(i)
        Next
        Return result
    End Function
#End Region

#Region "运算方法 "
    Private Function Calc(ByVal opt As String, ByVal expression As String) As Decimal()

        On Error Resume Next
        Dim mcalc As Calculator = New Calculator(expression, _data)
        Dim values As Decimal() = mcalc.Calculate()

        Dim v As Decimal = values(values.Length - 1)
        Dim r As Decimal = Decimal.Zero
        Select Case _opt
            ''---------------------------------------

            Case "ROUNDEX(" '外加函數

                If values.Length > 2 Then strMsg = "Calc" : MsgBox("表达式 " & _expression & " 中RoundEx函数需要一个或两个参数! ") ' Throw New Exception("表达式 " & _expression & " 中Round函数需要一个或两个参数! ")
                If values.Length = 1 Then
                    r = RoundEx(v, 0)
                Else
                    r = RoundEx(values(0), CType(values(1), Integer))
                End If
                ''----------------------------------------
            Case "("
                r = v
            Case "ROUND("
                If values.Length > 2 Then strMsg = "Calc" : MsgBox("表达式 " & _expression & " 中Round函数需要一个或两个参数! ") ' Throw New Exception("表达式 " & _expression & " 中Round函数需要一个或两个参数! ")
                If values.Length = 1 Then
                    r = Decimal.Round(v, 0)
                Else
                    r = Decimal.Round(values(0), CType(values(1), Integer))
                End If
            Case "TRUNC("
                If values.Length > 1 Then strMsg = "Calc" : MsgBox("表达式  " & _expression & " 中Trunc函数只需要一个参数! ") 'Throw New Exception("表达式  " & _expression & " 中Trunc函数只需要一个参数! ")
                r = Decimal.Truncate(v)
            Case "MAX( "
                If values.Length < 2 Then strMsg = "Calc" : MsgBox("表达式 " & _expression & " 中Max函数至少需要两个参数! ") 'Throw New Exception("表达式 " & _expression & " 中Max函数至少需要两个参数! ")
                r = values(0)
                Dim i As Integer
                For i = 1 To values.Length - 1
                    If values(i) > r Then r = values(i)
                Next
            Case "MIN("
                If values.Length < 2 Then strMsg = "Calc" : MsgBox("表达式  " & _expression & "中Min函数至少需要两个参数! ") 'Throw New Exception("表达式  " & _expression & "中Min函数至少需要两个参数! ")
                r = values(0)
                Dim i As Integer
                For i = 1 To values.Length - 1
                    If values(i) < r Then r = values(i)
                Next
            Case "ABS("
                If values.Length > 1 Then strMsg = "Calc" : MsgBox("表达式  " & _expression & " 中Abs函数只需要一个参数! ") 'Throw New Exception("表达式  " & _expression & " 中Abs函数只需要一个参数! ")
                r = Math.Abs(v)
            Case "SUM("
                Dim d As Decimal
                For Each d In values
                    r += d
                Next
            Case "AVERAGE( "
                Dim d As Decimal
                For Each d In values
                    r += d
                Next
                r = r / values.Length
            Case "IF("
                If values.Length <> 3 Then strMsg = "Calc" : MsgBox("表达式  " & _expression & " 中IF函数需要三个参数! ") 'Throw New Exception("表达式  " & _expression & " 中IF函数需要三个参数! ")
                If GetBoolean(values(0)) Then
                    r = values(1)
                Else
                    r = values(2)
                End If
            Case "NOT("
                If values.Length <> 1 Then strMsg = "Calc" : MsgBox("表达式  " & _expression & " 中NOT函数需要一个参数! ") 'Throw New Exception("表达式  " & _expression & " 中NOT函数需要一个参数! ")
                If GetBoolean(values(0)) Then
                    r = 0
                Else
                    r = 1
                End If
            Case "OR("
                If values.Length < 1 Then strMsg = "Calc" : MsgBox("表达式 " & _expression & " 中OR函数至少需要两个参数! ") 'Throw New Exception("表达式 " & _expression & " 中OR函数至少需要两个参数! ")
                Dim d As Decimal
                For Each d In values
                    If GetBoolean(d) Then Return New Decimal(0) {1}
                Next
            Case "AND("
                If values.Length < 1 Then strMsg = "Calc" : MsgBox("表达式  " & _expression & "中AND函数至少需要两个参数! ") 'Throw New Exception("表达式  " & _expression & "中AND函数至少需要两个参数! ")
                Dim d As Decimal
                For Each d In values
                    If Not GetBoolean(d) Then Return New Decimal(0) {Decimal.Zero}
                Next
                r = 1
            Case "SQRT("
                If values.Length <> 1 Then strMsg = "Calc" : MsgBox("表达式  " & _expression & "中SQRT函数需要一个参数! ") ' Throw New Exception("表达式  " & _expression & "中SQRT函数需要一个参数! ")
                r = CType(Math.Sqrt(CType(v, Double)), Decimal)
            Case "SIN("
                If values.Length <> 1 Then strMsg = "Calc" : MsgBox("表达式  " & _expression & "中Sin函数需要一个参数! ") 'Throw New Exception("表达式  " & _expression & "中Sin函数需要一个参数! ")
                r = CType(Math.Sin(CType(v, Double)), Decimal)
            Case "COS("
                If values.Length <> 1 Then strMsg = "Calc" : MsgBox("表达式 " & _expression & "中Cos函数需要一个参数! ") 'Throw New Exception("表达式 " & _expression & "中Cos函数需要一个参数! ")
                r = CType(Math.Cos(CType(v, Double)), Decimal)
            Case "TAN( "
                If values.Length <> 1 Then strMsg = "Calc" : MsgBox("表达式 " & _expression & "中Tan函数需要一个参数! ") 'Throw New Exception("表达式 " & _expression & "中Tan函数需要一个参数! ")
                r = CType(Math.Tan(CType(v, Double)), Decimal)
            Case "EXP("
                If values.Length <> 1 Then strMsg = "Calc" : MsgBox("表达式 " & _expression & "中Exp函数需要一个参数! ") 'Throw New Exception("表达式 " & _expression & "中Exp函数需要一个参数! ")
                r = CType(Math.Exp(CType(v, Double)), Decimal)
            Case "LOG("
                If values.Length > 2 Then strMsg = "Calc" : MsgBox("表达式 " & _expression & "中Log函数需要一个或两个参数! ") 'Throw New Exception("表达式 " & _expression & "中Log函数需要一个或两个参数! ")
                If values.Length = 1 Then
                    r = CType(Math.Log(CType(v, Double)), Decimal)
                Else
                    r = CType(Math.Log(CType(values(0), Double), CType(values(1), Double)), Decimal)
                End If
            Case "LOG10("
                If values.Length <> 1 Then strMsg = "Calc" : MsgBox("表达式 " & _expression & "中Log10函数需要一个参数! ") 'Throw New Exception("表达式 " & _expression & "中Log10函数需要一个参数! ")
                r = CType(Math.Log10(CType(v, Double)), Decimal)
        End Select
        Return New Decimal(0) {r}
    End Function

    Private Function GetBoolean(ByVal d As Decimal) As Boolean
        Return CType(d, Integer) = 1
    End Function

    Private Function Calc(ByVal opt As String, ByVal leftEx As String, ByVal rightEx As String) As Decimal()

        Dim r As Decimal = Decimal.Zero
        Dim left As Decimal = Decimal.Zero
        Dim right As Decimal

        Try
            left = Decimal.Parse(leftEx)
        Catch
            If opt <> "NEG" Then
                Try
                    left = Decimal.Parse(_data(leftEx))
                Catch
                    'Throw New Exception("表达式 " & _expression & " 中含有错误的格式: " & leftEx)
                    MsgBox("表达式 " & _expression & " 中含有错误的格式: " & leftEx)
                    strMsg = "Calc"
                    GoTo err1
                End Try
            End If
        End Try

        Try
            right = Decimal.Parse(rightEx)
        Catch
            Try
                right = Decimal.Parse(_data(rightEx))
            Catch
                ' Throw New Exception("表达式  " & _expression & "中含有错误的格式: " & leftEx)
                MsgBox("表达式  " & _expression & "中含有错误的格式: " & leftEx)
                strMsg = "Calc"
                GoTo err1
            End Try
        End Try

        Select Case _opt
            Case "NEG"
                r = Decimal.Negate(right)
            Case "+"
                r = left + right
            Case "-"
                r = left - right
            Case "*"
                r = left * right
            Case "/"
                r = left / right
            Case "%"
                r = Decimal.Remainder(left, right)
            Case "^"
                r = CType(Math.Pow(CType(left, Double), CType(right, Double)), Decimal)
            Case ","
                Return New Decimal(1) {left, right}
            Case "="
                r = IIf(left = right, 1, 0)
            Case "<>"
                r = IIf(left <> right, 1, 0)
            Case "<"
                r = IIf(left < right, 1, 0)
            Case ">"
                r = IIf(left > right, 1, 0)
            Case ">="
                r = IIf(left >= right, 1, 0)
            Case "<="
                r = IIf(left <= right, 1, 0)
        End Select
        Return New Decimal(0) {r}
err1:
    End Function

#End Region


    Public Function RoundEx(ByVal Number As Double, Optional ByVal nLen As Integer = 0)
        '取代Round的四舍五入
        On Error GoTo ErrRound
        Dim dblAdd As Double
        dblAdd = 10 ^ (-nLen - 1)
        If Number < 0 Then dblAdd = -dblAdd
        Number = Number + dblAdd
        RoundEx = Math.Round(Number, nLen)
        Exit Function
ErrRound:
        MsgBox("實時錯誤：" & Err.Number & vbCrLf & vbCrLf & Err.Description, vbExclamation, "錯誤提示")
    End Function

End Class

