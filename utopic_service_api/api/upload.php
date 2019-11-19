<?php
include_once($_SERVER['DOCUMENT_ROOT'] . '/utopic/class/database.php');
include_once($_SERVER['DOCUMENT_ROOT'] . '/utopic/functions/azure_json_return.php');
include_once($_SERVER['DOCUMENT_ROOT'] . '/utopic/functions/set_get_tag.php');
/*
$uploads_dir = $_SERVER['DOCUMENT_ROOT'] . '/utopic/files'; //'./files';
if ($_FILES["file"]["error"] == UPLOAD_ERR_OK) {
    $tmp_name = $_FILES["file"]["tmp_name"];
    $name = $_FILES["file"]["name"];
    move_uploaded_file($tmp_name, "$uploads_dir/$name");
*/

//database init

//$database=new Database('localhost','root','amarsql','utopic');
$database=new Database('localhost','root','Utopic10!','utopic_repo');
$database->connect();

echo "upload.php<br>";

include_once($_SERVER['DOCUMENT_ROOT'] . '/utopic/class/database.php');

$uploads_dir = $_SERVER['DOCUMENT_ROOT'] . '/utopic/files'; //'./files';
$temp_dir =$_SERVER['DOCUMENT_ROOT'] . '/utopic/temp_file';

if(isset($_POST['submit'])) {
    //this submit will get $user_id also
    if (isset($_FILES) && isset($_POST['user_id'])) {

        //get user id
        $user_id = $_POST['user_id'];

        //now file processing starts

        //image validators should be added here
        //print_r($_FILES);


        $tmp_name = $_FILES["file"]["tmp_name"];
        $name = $_FILES["file"]["name"];
        //move it to temp folder
        $val = move_uploaded_file($tmp_name, "$temp_dir/$name");
        if ($val == false) die("not moved to temp");


        //now validate image
        //getting json
        $azure_json = azure_json_return("http://scintilib.com/utopic/temp_file/" . $name);

        $decode_azure_json = json_decode($azure_json, true);
        //key value pair
        /*
          foreach ($azure_json as $key=>$value){
            echo $key. "<==>" .$value. "<br>";
        }
        */
        //print_r($decode_azure_json);
        //print_r( "CODE: ".$decode_azure_json['code']);

        if (array_key_exists('code', $decode_azure_json)) {
            if ($decode_azure_json['code'] == "InvalidImageFormat") die($decode_azure_json['code']); //and delete
        }
        //store the json info in database



        //generate hash of the image


        //sh1
        $sh1hash = sha1(file_get_contents("$temp_dir/$name"));

        //md5
        $md5hash = md5(file_get_contents("$temp_dir/$name"));


        $concated_hash = $sh1hash . $md5hash;


        //cross match with database

        //first check if the image belongs to user
        $sql = "SELECT id FROM photo WHERE u_id='$concated_hash'";
        $result = $database->query($sql);


        //if the image exists
        if ($result->num_rows != 0) {
            echo "alreday exists<br>";
            $result = $database->getArray($sql);
            print_r($result);
            $photo_id = $result[0]["id"];

            //checking relation
            $sql = "SELECT id,user_id FROM user_photo WHERE photo_id='$photo_id'";
            $result = $database->query($sql);

            if ($result->num_rows != 0) { //means the photo is already by someone
                $result = $database->getArray($sql);
                print_r($result);
                if ($result[0]['user_id'] == $user_id) { //if the photo was uploaded by the present user previously
                    //no upload
                    //delete from temp
                    unlink("$temp_dir/$name");
                } else {

                    //other user has uploaded, so establish relation with present user
                    $sql = "INSERT INTO user_photo(user_id,photo_id) VALUES('$user_id','$photo_id')";
                    $database->query($sql);

                }

            }
        } else {
            echo "being inserted<br>";
            //insert image in image table

            //first get the meta info as creation date is needed
            $exif = exif_read_data("$temp_dir/$name", 'IFD0');
            //from php.net exif_read_data manual
            if (@array_key_exists('DateTime', $exif)) {
                $capture_time = $exif['DateTime'];
            }
            //after you told not to push null
            //can't fix this issue, so for the time being NULL
            else {
                $capture_time = NULL;
            }

            //insert the json and store it in a database
            //photo_json is not a relationship table, it just stores the json, and the relation is created in photo table json_id column
            $sql = "INSERT INTO photo_json(json) VALUES('$azure_json')";
            $database->query($sql);
            $json_id = $database->getLastInsertId();

            //size is needed for sql
            $image_size = $_FILES['file']['size'];
            //move to upload dir
            $val = rename("$temp_dir/$name", "$uploads_dir/$name");
            print_r($decode_azure_json);
            //get caption of the photo
            $caption=$decode_azure_json['description']['captions']['0']['text'];

            if ($val == false) die("not moved to upload dir");
            //get size for db
            //now the insertion
            $url = "$uploads_dir" . "/" . "$name"; //needed for insertion
            //if capture time doesn't exist, then default timestamp
            if ($capture_time == NULL) {
                $sql = "INSERT INTO photo(sha1, md5, u_id, size,  image_url,json_id,caption) VALUES ('$sh1hash','$md5hash','$concated_hash','$image_size','$url','$json_id','$caption')";
            } else $sql = "INSERT INTO photo(sha1, md5, u_id, size, capture_time, image_url,json_id,caption) VALUES ('$sh1hash','$md5hash','$concated_hash','$image_size','$capture_time','$url','$json_id','$caption')";
            //now insert
            $result = $database->query($sql);
            //get last id
            $photo_id = $database->getLastInsertId($sql);


            $sql = "INSERT INTO user_photo(user_id, photo_id) VALUES ('$user_id','$photo_id')";
            $database->query($sql);

            //illiuminast, I insist you not to delete comments, ofcourse you can delete all the echoes and print_r

            //now get the tags and push them in the database
            //using the photo_id
            //there will be two tags
            //1.tags based on score
            //2.description[tags]

            //after pushing, relation will be created, necessary for search.php
            //and also, caption is going to be appended in the photo table

            //first thing first, get tag, and set tag


            //first thing first, get tag, and set tag
            //gettag()-will get id of the tag upon availability, or else create and return id
            //settag()-will create relationship on table photo_tag
            //set tag running for only tags
            $keys=array_keys($decode_azure_json['tags']);
            for($i=0;$i<count($decode_azure_json['tags']);$i++){
              $tag=$decode_azure_json['tags'][$keys[$i]]['name'];
              echo $tag."<br>";
              set_tag($tag,$photo_id);
            }
            //set tag running for description_tags
            foreach ($decode_azure_json['description']['tags'] as $key=>$value){
                set_tag($value,$photo_id);
                echo $value."<br>";
            }


            exit();

        }
    } else {
        echo "post error";
    }
}
else {
    echo "request error";
}
//last er line er comment
?>