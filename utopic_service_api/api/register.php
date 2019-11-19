
<?php
include_once($_SERVER['DOCUMENT_ROOT'] . '/utopic/class/database.php');
include_once($_SERVER['DOCUMENT_ROOT'] . '/utopic/class/user.php');
include_once($_SERVER['DOCUMENT_ROOT'] . '/utopic/functions/validator.php');
include_once($_SERVER['DOCUMENT_ROOT'] . '/utopic/functions/init_database.php');
$database = init_database();
$database->connect();
$user = new User($database);
$username = $database->escape($_POST['username']);
$password = $_POST['password'];
$phone = $database->escape($_POST['phone']);
$email = $database->escape($_POST['email']);
//("Arnika", "4621", "+8801750252963", "arnikaroyrumu@gmail.com", "Arnika Roy Rumu", "1997-09-13")
if (isset($_POST['username']) &&
    isset($_POST['password']) &&
    isset($_POST['phone']) &&
    isset($_POST['email']))
    {
        echo "pont 1";
    $user->register(
        $username,
        $password,
        $phone,
        $email
    );
    echo "pont 2";
}
?>