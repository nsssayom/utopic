<?php
include_once($_SERVER['DOCUMENT_ROOT'] . '/utopic/functions/response.php');
class Database
{
    protected $link;
    protected $hostname;
    protected $username;
    protected $password;
    protected $database;

    function __construct($host, $user, $pass, $database)
    {
        ini_set('display_errors', 1);
        ini_set('display_startup_errors', 1);
        error_reporting(E_ALL);

        $this->hostname = $host;
        $this->username = $user;
        $this->password = $pass;
        $this->database = $database;
        $this->link = NULL;
        $this->connect();
    }

    function connect()
    {
        /* Connect To MySQL */
        $mysql = new mysqli($this->hostname, $this->username, $this->password, $this->database);
        if ($mysql->connect_error) {
            $data = array();
            $data['status'] = "400";
            $result = json_encode(array($data));
            print_r($result);
            return false;
        } else {
            $this->link = $mysql;
            return true;
        }
    }

    function query($sql)
    {
        /* Returns MySQL Query */
        //for handling database error
        $result = $this->link->query($sql) or die("query error: $sql");          // $result = false;
        return $result;


    }

    function escape($data)
    {
        // Escape String
        return $this->link->real_escape_string($data);
    }

    function getArray($sql)
    {
        $mq = $this->query($sql);
        if (!$mq) {
            //for handling database error
            response_database_error();
        }

        $result = array();
        while ($mfa = $mq->fetch_array(MYSQLI_ASSOC)) {
            $result[] = $mfa;
        }
        return $result;
    }

    function getLink()
    {
        return $this->link;
    }

    function getLastInsertId(){
        $last_id = $this->link->insert_id;
        if($last_id==NULL) die("no last id");
        return $last_id;
    }

}