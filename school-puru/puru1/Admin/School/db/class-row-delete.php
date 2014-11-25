<?php
/*
*Code to delete the item.
*/	
	include('../../db.php');
	$mdm= $_POST['mdm'];
	$std=$_POST['std'];
	mysqli_query($con,"DELETE FROM sch_class where Medium='".trim($mdm)."' AND Std='".trim($std)."'");
	echo json_encode("DELETE FROM Sch_class where medium='".trim($mdm)."' AND Std='".trim($std)."'");
?>