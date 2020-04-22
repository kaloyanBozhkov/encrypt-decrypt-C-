<?php 
require("connection.php");
$query = $_POST["query"];
$i = explode(";", $query);
$operationType = $_POST['operationType'];
if($operationType == "read"){
    $sql = "(SELECT id FROM `glyndwr_task3_users` WHERE initials = '".$i[0]."') UNION (SELECT generatedNumberSet as 'id' FROM `glyndwr_task3_keywords` WHERE keyword = '".$i[1]."' and date='".$i[2]."')";
    $result = mysqli_query($link, $sql);
    $number = mysqli_num_rows($result);
    if ($number >= 2) {
            $code = "";
            while ($row=mysqli_fetch_array($result)) {
                $code = $row["id"]; //loops twice on second time sets code to generatedNumberSet
            }
       echo($code);
    }else{
        echo("none");
    }
}else{
    $sql = "INSERT INTO `glyndwr_task3_users`(`id`, `initials`, `email`) VALUES ('','".mysqli_real_escape_string($link, $i[0])."','".mysqli_real_escape_string($link, $i[1])."')";
    if (mysqli_query($link, $sql)) {
        echo("done");
    }else{
        echo("issue");
    }
}
