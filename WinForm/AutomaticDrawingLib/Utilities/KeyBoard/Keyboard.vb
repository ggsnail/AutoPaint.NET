﻿Public Class Keyboard
    Private Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Int32, ByVal dx As Int32, ByVal dy As Int32, ByVal cButtons As Int32, ByVal dwExtraInfo As Int32)
    Private Declare Function SetCursorPos Lib "user32" (ByVal x As Integer, ByVal y As Integer) As Integer
    Private Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)
    Private Declare Function MapVirtualKey Lib "user32" Alias "MapVirtualKeyA" (ByVal wCode As Integer, ByVal wMapType As Integer) As Integer
    Private Declare Function GetAsyncKeyState Lib "user32 " (ByVal vKey As Integer) As Integer
    ''' <summary>
    ''' 鼠标左键按下或弹起
    ''' </summary>
    Public Shared Sub MouseDownOrUp(type As Boolean, interval As Integer)
        If type Then '按下
            mouse_event(&H2, 0, 0, 0, IntPtr.Zero)
        Else '弹起
            mouse_event(&H4, 0, 0, 0, IntPtr.Zero)
        End If
        System.Threading.Thread.Sleep(interval)
    End Sub
    ''' <summary>
    ''' 鼠标移动
    ''' </summary>
    Public Shared Sub MouseMove(x As Integer, y As Integer, interval As Integer)
        SetCursorPos(x, y)
        System.Threading.Thread.Sleep(interval)
    End Sub
    ''' <summary>
    ''' 发送一组按键
    ''' </summary>
    Public Shared Sub SendString(str As String, interval As Integer)
        Dim strArr() As String = Split(str, ",")
        For Each SubStr In strArr
            If SubStr.First = "#" Then
                System.Threading.Thread.Sleep(CInt(SubStr.Substring(1)))
            Else
                For Each SubChar As Char In SubStr
                    VirtualKeyDown(Asc(SubChar))
                    System.Threading.Thread.Sleep(interval)
                    VirtualKeyUp(Asc(SubChar))
                Next
            End If
        Next
    End Sub
    ''' <summary>
    ''' 发送一组扩展的按键
    ''' </summary>
    Public Shared Sub SendStringEx(str As String, interval As Integer, ParamArray extra() As VirtualKey)
        For Each SubKey In extra
            VirtualKeyDown(SubKey)
        Next
        SendString(str, interval)
        For Each SubKey In extra
            VirtualKeyUp(SubKey)
        Next
    End Sub
    ''' <summary>
    ''' 发送单个按键
    ''' </summary>
    Public Shared Sub SendKey(vKey As VirtualKey， interval As Integer)
        VirtualKeyDown(vKey)
        System.Threading.Thread.Sleep(interval)
        VirtualKeyUp(vKey)
    End Sub
    ''' <summary>
    ''' 同时发送两个按键
    ''' </summary>
    Public Shared Sub SendCouple(vKey1 As VirtualKey, vKey2 As VirtualKey, interval As Integer)
        VirtualKeyDown(vKey1)
        VirtualKeyDown(vKey2)
        System.Threading.Thread.Sleep(interval)
        VirtualKeyUp(vKey1)
        VirtualKeyUp(vKey2)
    End Sub
    ''' <summary>
    '''  获取A~Z的激活按键
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetActiveLetterKey() As Byte
        For i = 65 To 90
            If CurrentKeyState(i) = True Then Return i
        Next
        Return 0
    End Function
    ''' <summary>
    ''' 获取指定按键的状态
    ''' </summary>
    Private Shared Function CurrentKeyState(ByVal keyCode As Byte) As Boolean
        Dim temp As Integer = GetAsyncKeyState(keyCode)
        Return (IIf(temp = -32767, True, False))
    End Function
    ''' <summary>
    ''' 按下指定按键
    ''' </summary>
    Public Shared Sub VirtualKeyDown(vKey As VirtualKey)
        keybd_event(vKey, MapVirtualKey(vKey, 0), &H1 Or 0, 0)
    End Sub
    ''' <summary>
    ''' 松开指定按键
    ''' </summary>
    Public Shared Sub VirtualKeyUp(vKey As VirtualKey)
        keybd_event(vKey, MapVirtualKey(vKey, 0), &H1 Or &H2, 0)
    End Sub
End Class
