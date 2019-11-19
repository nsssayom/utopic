<?php
function response_ok(){
    $data = array();
    $data['status'] = "100";
    echo json_encode(array($data));
    exit();
}
function response_database_error(){
    $data = array();
    $data['status'] = "400";
    echo json_encode(array($data));
    exit();
}
function response_token_creation_failed(){
    $data = array();
    $data['status'] = "270";
    echo json_encode(array($data));;
    exit();
}
function response_token($token){
    $data = array();
    $data ['status'] = "100";
    $data ['token'] = $token;
    echo json_encode(array($data));
    exit();
}
function response_token_update_failed(){
    $data = array();
    $data['status'] = "275";
    echo json_encode(array($data));
    exit();
}
function response_token_expired(){
    $data = array();
    $data['status'] = "280";
    echo json_encode(array($data));
    exit();
}
function response_invalid_token(){
    $data = array();
    $data['status'] = "285";
    echo json_encode(array($data));
    exit();
}
function response_username_not_available(){
    $data = array();
    $data['status'] = "301";
    echo json_encode(array($data));
    exit();
}
function response_invalid_email(){
    $data = array();
    $data['status'] = "302";
    echo json_encode(array($data));
    exit();
}
function response_email_not_available(){
    $data = array();
    $data['status'] = "303";
    echo json_encode(array($data));
    exit();
}
function response_invalid_phone_number(){
    $data = array();
    $data['status'] = "304";
    echo json_encode(array($data));
    exit();
}
function response_phone_number_not_available(){
    $data = array();
    $data['status'] = "305";
    echo json_encode(array($data));
    exit();
}
function response_email_cannot_be_verified(){
    $data = array();
    $data['status'] = "310";
    echo json_encode(array($data));
    exit();
}
function response_phone_cannot_be_verified(){
    $data = array();
    $data['status'] = "311";
    echo json_encode(array($data));
    exit();
}
function response_invalid_dateofbirth(){
    $data = array();
    $data['status'] = "312";
    echo json_encode(array($data));
    exit();
}
function response_invalid_request(){
    $data = array();
    $data['status'] = "330";
    echo json_encode(array($data));
    exit();
}
function response_invalid_user(){
    $data = array();
    $data['status'] = "255";
    echo json_encode(array($data));
    exit();
}
function response_wrong_password(){
    $data = array();
    $data['status'] = "258";
    echo json_encode(array($data));
    exit();
}
function response_entity_unavailable(){
    $data = array();
    $data['status'] = "259";
    echo json_encode(array($data));
    exit();
}
function response_permission_invalid(){
    $data = array();
    $data['status'] = "260";
    echo json_encode(array($data));
    exit();
}