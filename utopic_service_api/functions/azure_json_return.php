<?php

function azure_json_return($imageFilePath)
{
    require_once 'HTTP/Request2.php';

    $request = new Http_Request2('https://southeastasia.api.cognitive.microsoft.com/vision/v1.0/analyze');
    $url = $request->getUrl();

    $headers = array(
        // Request headers
        'Content-Type' => 'application/json',
        'Ocp-Apim-Subscription-Key' => 'dd959986b2184ce59a4a8a90a8cab17c',
    );

    $request->setHeader($headers);

    $parameters = array(
        // Request parameters
        'visualFeatures' => 'Categories,Tags,Description,Faces,ImageType,Color,Adult',
        'details' => 'Celebrities,Landmarks',
        'language' => 'en',
    );

    $url->setQueryVariables($parameters);

    $request->setMethod(HTTP_Request2::METHOD_POST);

// Request body
    $request->setBody("{ \"url\":\"" . $imageFilePath . "\"}");

    try {
        $response = $request->send();
        //returning for image validation and database entry
        return ($response);
    } catch (HttpException $ex) {
        echo $ex;
    }
}

?>
