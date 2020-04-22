<?php
require("connection.php");
$operationType = $_POST['operationType'];
$q = $_POST["query"];
if($operationType == "read"){
    $sql = "SELECT * FROM `glyndwr_task3`";
    if(strpos($q, "keyword:") !== false){
        $sql .= " WHERE generatedNumberSet LIKE '%".str_replace("keyword:", "", $q)."%'";
    }elseif(strpos($q, "date:") !== false){
        $sql .= " WHERE date = '".str_replace("date:", "", $q)."'";
    }elseif(strpos($q, "between:") !== false){
        $dates = explode("?", str_replace("between:", "", $q));
         $sql .= " WHERE date >= '".$dates[0]."' AND date <= '".$dates[1]."'";
    }elseif(strpos($q, "agent:") !== false){
         $sql .= " WHERE operatorInitials = '".str_replace("agent:", "", $q)."'";
    }
    $sql .= " ORDER BY id DESC;";
    $string = "";
    $result = mysqli_query($link, $sql);
    $number = mysqli_num_rows($result);
    if ($number > 0) {
            while ($row=mysqli_fetch_array($result)) {
                $string .= $row["id"] . ";".  $row["date"] . ";" . $row["time"] . ";".  $row["message"] . ";". $row["generatedNumberSet"] . ";". $row["operatorInitials"] . ";". ucfirst($row["operationType"]) . "|"; //loops twice on second time sets code to generatedNumberSet
            }
       echo(substr($string, 0, strlen($string) - 1));
    }else{
        echo("none");
    }
}else{
    $details = explode("|", $q);
    $sql = "INSERT INTO `glyndwr_task3`(`id`, `date`, `time`, `message`, `generatedNumberSet`, `operatorInitials`, `operationType`) VALUES ('','".mysqli_real_escape_string($link, $details[0])."','".mysqli_real_escape_string($link, $details[1])."','".mysqli_real_escape_string($link, $details[2])."','".mysqli_real_escape_string($link, $details[3])."','".mysqli_real_escape_string($link, $details[4])."', '".mysqli_real_escape_string($link, $details[5])."')";
    if (mysqli_query($link, $sql)) {
       echo("added");
    }else{
        echo("issue");
    }
}
?>
