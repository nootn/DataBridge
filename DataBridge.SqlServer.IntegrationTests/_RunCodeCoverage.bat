"..\..\..\packages\OpenCover.4.6.519\Tools\OpenCover.Console.exe" -target:"..\..\..\packages\NUnit.Runners.Net4.2.6.4\tools\nunit-console.exe" -targetargs:"/nologo DataBridge.SqlServer.IntegrationTests.dll /noshadow" -filter:"+[DataBridge.Core]DataBridge.Core* +[DataBridge.SqlServer]DataBridge.SqlServer* -[DataBridge.SqlServer]DataBridge.SqlServer.Configuration* -[DataBridge.SqlServer]DataBridge.SqlServer.Extensions*" -excludebyattribute:"System.CodeDom.Compiler.GeneratedCodeAttribute" -register:user -output:"_CodeCoverageResult.xml"

"..\..\..\packages\ReportGenerator.2.4.5.0\tools\ReportGenerator.exe" "-reports:_CodeCoverageResult.xml" "-targetdir:_CodeCoverageReport"