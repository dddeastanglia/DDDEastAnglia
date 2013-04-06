function Get-ScriptDirectory {
    $invocation = (Get-Variable MyInvocation -Scope 2).Value
    return Split-Path $invocation.MyCommand.Path
}