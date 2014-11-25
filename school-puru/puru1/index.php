 <?php
	session_start();
	if(!isset($_SESSION['uname'])){
	include('attach/header_ind.php');
?>
<div class="span-left" style="visibility:hidden;width:100px;">
</div>
<div class="span-right">
	<div class="main-container">
		<div class="post-header">
			<span>Login </span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<form class="login-main" method="post" action="">
					<div class="login-box">
						<div class="login-row">
							<span class="login-text">
								User Type:
							</span>
							<span class="login-input">
							<select name="login-utyp">
								<option value="default">Select One</option>
								<option value="Admin">Admin</option>
								<option value="College">College</option>
								<option value="School">School</option>
							</select>
							</span>
						</div>
						<div class="login-row">
							<span class="login-text">
								User name:
							</span>
							<span class="login-input">
							<input type="text" name="login-uname" autocomplete="off">
							</span>
						</div>
						<div class="login-row">
							<span class="login-text">
								Password:
							</span>
							<span class="login-input">
							<input type="password" name="login-pwd" autocomplete="off">
							</span>
						</div>
						<div class="login-row">
						<button class="login-sub">
							<span>
								login
							</span>
						</button>
						</div>
					</div>
				</form>
			</div>
		</div>
	</div>
</div>
 <?php
	include('attach/footer_ind.php');
	}
	else{
		header('Location:'.$_SESSION['type']);
	}
?>
