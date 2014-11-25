<?php
	session_start();
	include('db.php');
	parse_str($_POST['login'], $set);
	$check=mysqli_fetch_row(mysqli_query($con,"select uname,name,type from admin_sch where uname='".$set['login-uname']."' AND pwd='".md5($set['login-uname'])."' AND type='".$set['login-utyp']."'"));
	//echo("select uname,name,type from admin_sch where uname='".$set['login-uname']."' AND pwd='".md5($set['login-uname'])."' AND type='".$set['login-utyp']."'");
	if($check){
		$arr['status']='suceed';
		$arr['type']=$check[2];
		$_SESSION['uname']=$check[0];
		$_SESSION['name']=$check[1];
		$_SESSION['type']=$check[2];
		echo json_encode($arr);
		exit;
	}
	else{
		$arr['status']='failed';
		session_destroy();
		echo json_encode($arr);
		exit;
	}
?>