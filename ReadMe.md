# ConvertCodeFromUTF8
一个转换代码中UTF8编码字符为8进制字符的工具。  
如果代码中有中文或其他特殊字符，可以在编译前用本工具转换，避免warning C4566。
# 使用方法
`dotnet ConvertCodeFromUTF8.dll 文件夹路径 [文件后缀名1] [文件后缀名2] ...`  
例如，转换在`D:\build`处的所有c++代码文件：  
`dotnet ConvertCodeFromUTF8.dll D:\build *.h *.cpp`  