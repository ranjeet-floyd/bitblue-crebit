<?php 
	include('../../db.php');
	parse_str($_POST['header_name'],$name);
	
	mysqli_query($con,"UPDATE info SET Name='".$name['header-name']."' where key_p='008'");
	$arr['status']='Updated Successfully';
	echo json_encode($arr);
	exit;
?>