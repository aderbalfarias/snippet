<?php 
// Creating New Session
// ==========================
session_start();
/*session is started if you don't write this line can't use $_Session  global variable*/
$_SESSION["newsession"]=$value;

// Getting Session
// ==========================
session_start();
/*session is started if you don't write this line can't use $_Session  global variable*/
$_SESSION["newsession"]=$value;
/*session created*/
echo $_SESSION["newsession"];
/*session was getting*/

// Updating Session
// ==========================
session_start();
/*session is started if you don't write this line can't use $_Session  global variable*/
$_SESSION["newsession"]=$value;
/*it is my new session*/
$_SESSION["newsession"]=$updatedvalue;
/*session updated*/

// Deleting Session
// ==========================
session_start();
/*session is started if you don't write this line can't use $_Session  global variable*/
$_SESSION["newsession"]=$value;
unset($_SESSION["newsession"]);
/*session deleted. if you try using this you've got an error*/
?>