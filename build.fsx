#r "paket: groupref Build //"

#load ".fake/build.fsx/intellisense.fsx"
open Fake
open Fake.Core
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Fake.DotNet
//open Fake.DotNet.Testing

let appSrc = "src"


Fake.Core.Target.create "Build" (fun _ ->
    DotNet.build (fun p -> { p with Configuration = DotNet.BuildConfiguration.Debug}) appSrc |> ignore
)

Fake.Core.Target.create "RunTests" (fun _ ->
    ()
    //TODO
    // DotNet.test 
    //     (fun p ->
    //              { p with
    //                   Configuration = DotNet.BuildConfiguration.Debug
    //                   Logger = Some "trx;logfilename=../../../results.trx"
    //               }
    //     )
    //     testsSrc
)

Fake.Core.Target.create "Run" (fun _ ->
    DotNet.exec (fun p -> {p with WorkingDirectory = appSrc}) "run" "" |> ignore
)

Fake.Core.Target.create "Publish" (fun _ ->
    DotNet.publish  (fun p -> { p with Configuration = DotNet.BuildConfiguration.Release ; OutputPath = Some "../../deploy" }) appSrc |> ignore
    File.delete "deploy/appsettings.development.json"
)

Fake.Core.Target.create "Clean" (fun _ -> 
    !! "src/*/bin"
    ++ "src/*/obj"
    ++ "tests/*/bin"
    ++ "tests/*/obj"
    ++ "deploy"
    |> Shell.deleteDirs
)

Fake.Core.Target.create "AssemblyInfo" <| fun _ ->
    let props = Fake.Tools.GitVersion.generateProperties (fun x -> { x with ToolPath = "packages/build/GitVersion.CommandLine/tools/GitVersion.exe" })
    for projFile in !! ("src/**/*.fsproj") do
        let file = 
            projFile 
            |> Fake.IO.DirectoryInfo.ofPath
            |> (fun i -> i.Parent) 
            |> (fun i -> sprintf "%s\AssemblyInfo.fs" i.FullName)
            
        AssemblyInfoFile.createFSharp file [ 
                Fake.DotNet.AssemblyInfo.InformationalVersion props.InformationalVersion
                Fake.DotNet.AssemblyInfo.Version props.AssemblySemVer
            ]
        |> ignore

Fake.Core.Target.create "CleanBinObj" (fun _ -> 
    !! "src/*/bin"
    ++ "src/*/obj"
    ++ "tests/*/bin"
    ++ "tests/*/obj"
    |> Shell.deleteDirs
)


"Clean" ==> "AssemblyInfo" ==>  "Publish"
"RunTests" ==> "Publish"


// start build
Fake.Core.Target.runOrDefault "Build"