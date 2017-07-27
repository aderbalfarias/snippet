<?php 
//$sql = select no banco

if (mysql_num_rows($sql)>0){
session_start();
$_SESSION['usuario'] = $linha['id_usuario'];
header ("Location: area_logada.php");
}
?>


<?php
if(!isset($_SESSION['usuario'])){
	header ("Location: inicio.php");
}
?>

<?php
session_start();
session_destroy();
header ("Location: inicio.php");
?>
