<?php
include_once($_SERVER['DOCUMENT_ROOT'] . '/utopic/class/database.php');
include_once($_SERVER['DOCUMENT_ROOT'] . '/utopic/functions/set_get_tag.php');

function search($tag){
    $tag_id=get_tag($tag);
    //get url using join
    $sql="SELECT photo.url FROM photo  JOIN photo_tag ON photo_tag.photo_id=photo.id WHERE photo_tag='$tag_id'";

    $result=$database->getArray($sql);

}