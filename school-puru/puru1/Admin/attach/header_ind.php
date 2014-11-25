<?php
	session_start();
	if($_SESSION['type']=='Admin'){
	include('../db.php');
?>
<html>
	<head>
		<meta name="viewport" content="width=device-width, initial-scale=1.0">
		<link rel="stylesheet" type="text/css" href="../css/index.css"/>
	</head>
	<body>
	<div class="wrapper">
		<div class="nav-bar">
			<div class="nav-bar-inner">
				<div class="menu-container">
					<h3>Dextro</h3>
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

			<div class="row">	
				<?php			}
		else{
			header('Location: ../');
		}
		?>