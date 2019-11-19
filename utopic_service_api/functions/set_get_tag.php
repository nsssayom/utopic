<?php
include_once($_SERVER['DOCUMENT_ROOT'] . '/utopic/class/database.php');

function get_tag($tag){

    //init_database is necessary

    $database=new database('localhost','root','Utopic10!','utopic_repo');
    $database->connect();

    $sql="SELECT id FROM tag WHERE name='$tag'";
    $result=$database->query($sql);
    //if not available, insert and return id
    if($result->num_rows==0){

        $sql="INSERT INTO tag(name) VALUES ('$tag')";

        $database->query($sql);
        $result=$database->getLastInsertId();
        return $result;

    }
    //else simply return the id
    else {
        $result=$database->getArray($sql);
        return $result['0']['id'];
    }

}

function set_tag($tag,$photo_id){
    //again init_database
    $database=new database('localhost','root','Utopic10!','utopic_repo');
    $database->connect();

  $tag_id=get_tag($tag);
    $sql="INSERT INTO photo_tag(photo_id,tag_id) VALUES('$photo_id','$tag_id')";
    $database->query($sql);
}
?>