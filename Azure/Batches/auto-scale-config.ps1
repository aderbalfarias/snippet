$MaxNodes = 20;

$CurrentActiveTasks = $ActiveTasks.GetSample(1);
$CurrentRunningTasks = $RunningTasks.GetSample(1);
$TotalTasks=$CurrentActiveTasks+$CurrentRunningTasks;
$Nodes = min($TotalTasks, $MaxNodes)
$TargetDedicated = $Nodes
