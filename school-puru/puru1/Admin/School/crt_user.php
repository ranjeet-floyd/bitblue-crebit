 <?php
	include('../attach/header_sch.php');
?>
<div class="span-right">
	<div class="main-container">
		<div class="post-header">
			<span>Create New User</span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<form class="adm_cusr" method="POST" action="">
					
					<div class="box-left">
					     Username: <input  type="text" name="adm_cusr_uname">
					</div>
					<div class="box-right">
						 Password: <input  type="Password" name="adm_cusr_pwd">
					</div>
					<div class="box-left">
						 User Type: <select name="adm_cusr_type">
							<option value="default">Select One</option>
							<option value="Admin">Admin</option>
							<option value="College">College</option>
							<option value="School">School</option>
						 </select>
					</div>
					<div class="box-right">
						Name:<input  type="text" name="adm_cusr_name">
					</div>
					<div class="box-left">
						Phone No.:<input  type="text" name="adm_cusr_pnum">
					</div>
					<div class="box-right">
						<button class="adm_cusr_sub"><span>Create</span></button>
					</div>
				</form>
			</div>
		</div>
	</div>
</div>
 <?php
	include('../attach/footer_sch.php');
?>