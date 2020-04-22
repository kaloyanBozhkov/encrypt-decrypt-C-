<?php
require("connection.php");
header('Content-Type: text/xml');
$type = $_POST["operationType"];
if ($type == "read") {
    $div = '<?xml version="1.0" encoding="UTF-8"?><poems>';
    $sql = "SELECT * FROM `glyndwr_task3_poems` WHERE id > 0 ORDER BY id ASC";
    $result = mysqli_query($link, $sql);
    $number = mysqli_num_rows($result);
    if ($number != 0) {
        while ($row = mysqli_fetch_array($result)) {
            $div .= "
            <poem id='" . $row["id"] . "'>
            <id>" . $row["id"] . "</id>
            <title>" . $row["title"] . "</title>
            <body>" . $row["poem"] . "</body>
            </poem>";
        }
        echo ($div . "</poems>");
    } else {
        echo ("No poems added yet.");
    }
} elseif ($type == "write") {
    $query = $_POST["query"];
    $i = explode("|", $query);
    $sql = "INSERT INTO `glyndwr_task3_poems`(`id`, `poem`, `title`) VALUES ('','" . mysqli_real_escape_string($link, $i[1]) . "','" . mysqli_real_escape_string($link, $i[0]) . "')";
    if (mysqli_query($link, $sql)) {
        echo ("done");
    } else {
        echo ("issue");
    }
} else {
    echo ("operationType wrong");
}
