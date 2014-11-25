<?php
	session_start();
	if($_SESSION['type']=='Admin'){
	include('../db.php');
	$header_query=mysqli_query($con,'SELECT name from info where Key_p="008"');
	$header_name=mysqli_fetch_row($header_query);
?>
<html>
	<head>
		<meta name="viewport" content="width=device-width, initial-scale=1.0">
		<link rel="stylesheet" type="text/css" href="../../css/index.css"/>
	</head>
	<body>
	<div class="wrapper">
		<div class="nav-bar">
			<div class="nav-bar-inner">
				<div class="menu-container">
					<h3><?php echo $header_name[0];?></h3>
				</div>
				<div class="sub-container">
					<ul class="sub-container-menu">
						<li class="header-menu">
						Hello, <?php echo $_SESSION['uname'];?>
						</li>
						<li class="header-menu">
						Account Type:<?php echo $_SESSION['type'];?>
						</li>
						<li class="header-menu logout-span">
						logout
						</li>
					</ul>
				</div>
			</div>
		</div>			
		<div class="cover">	
			<div class="row">	
				<div class="span-left">
					<ul class="left-menu">
						 <a class="left-menu-link" href="index.php"><li class="left-menu-li" ><span>Home</span></li></a>
						 <a class="left-menu-link" href="class.php"><li class="left-menu-li"><span>Create New Class</span></li></a>
						 <a class="left-menu-link" href="set_fee.php"><li class="left-menu-li"><span>Set Class Fees</span></li></a>
						 <a class="left-menu-link" href="update_std.php"><li class="left-menu-li"><span>Update Student Info</span></li></a>
						 <a class="left-menu-link" href="tran.php"><li class="left-menu-li"><span>Transaction</span></li></a>
						 <a class="left-menu-link" href="src_std.php"><li class="left-menu-li"><span>Search Student</span></li></a>
						 <a class="left-menu-link"href="crt_user.php"><li class="left-menu-li"><span>Create New User</span></li></a>
						 <a class="left-menu-link"href="Edit_pro.php"><li class="left-menu-li"><span>Edit Account</span></li></a>
					 </ul>
				</div>
		<?php			}
		else{
			header('Location: ../');
		}
		?>