<?php 
require("connection.php");
$query = $_POST["query"];
$content = explode("|", $query);
$operationType = $_POST["operationType"];
if($operationType == "read"){
    $query = "SELECT id FROM `glyndwr_task3_keywords` WHERE date = '" . mysqli_real_escape_string($link, $content[0]) . "';";
    $result = mysqli_query($link, $query);
    $number = mysqli_num_rows($result);
    if ($number != 0) {
        echo("exists");   
    }else{
        echo("unknown");
    }
}else{
    if($operationType == "write"){
        $query = "INSERT INTO `glyndwr_task3_keywords` VALUES('', '". mysqli_real_escape_string($link, $content[0]) ."', '". mysqli_real_escape_string($link, $content[1]) . "', '". mysqli_real_escape_string($link, $content[2]) . "');";
    }elseif($operationType == "delete"){
        $query = "DELETE FROM `glyndwr_task3_keywords` WHERE date = '" . mysqli_real_escape_string($link, $content[0]) ."';";
    }
    if(mysqli_query($link, $query)){
        echo("success");
    }else{
        echo("issue");
    }
}
?>