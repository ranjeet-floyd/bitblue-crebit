
 <?php
	include('../attach/header_clg.php');	
	$query=mysqli_query($con,"select * from clg_class order by  medium,course,std");
	$i=0;
	?>
	<div class="span-right">
		<div class="main-container">
			<div class="post-header">
				<span>View Existing Class</span>
			</div>
			<div class="post-content">
				<div class="post-text">
	<?php
	if(mysqli_num_rows($query)!=0){
	?>
	
					<ul class="table-view">
						<li class="table-view-header" style="width:100px">
							Medium
						</li>
						<li class="table-view-header" style="width:100px;">
							Course
						</li>
						<li class="table-view-header" style="width:100px;">
							Year
						</li>
						<li class="table-view-header" style="width:100px;">
							Division
						</li>
						<li class="table-view-header" style="width:100px;">
							Actual Strength
						</li>
						
							
					</ul>	
					<?php	
						while($result=mysqli_fetch_row($query)){
							$i++;
							//	print_R($result);
					?>
					<ul class="table-view ">
					
						<li style="width:100px">
							<?php echo $result[0]; ?>
						</li>
						<li  style="width:100px;">
							<?php echo $result[1]; ?>
						</li>
						<li style="width:100px;">
							<?php echo $result[2]; ?>
						</li>
						<li  style="width:100px;">
							<?php echo $result[3]; ?>
						</li>
						<li style="width:100px;" >
							<?php
								$count=mysqli_num_rows(mysqli_query($con,"select * from user_sch where Medium='".$result[0]."' AND std='".$result[1]."'"));
								echo $count;
							?>
						</li>
					</ul>
					<?php
						}
	
				?>
				<div class="class-view-result">
				</div>
	<?php
	}
	else{
	?>
	NO class created
	<?php
	}
	?>
		</div>
		</div>
	</div>
			
	<div class="main-container">
		<div class="post-header">
			<span>Create Class</span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<form class="adm_clg_cls"  method="post">
					<div class="box-left">
						 Medium:<select name="adm_clg_cls_mdm">
							 <option value="default">Select One</option>
							 <option value="English">English</option>
							 <option value="Marathi">Marathi</option>
						</select>
					</div>
					<div class="box-right">
						 Course:<select name="adm_clg_cls_cor">
							 <option value="default">Select One</option>
							  <option value="B.A.">B.A.</option>
							 <option value="B.Com">B.Com</option>
							 <option value="B.Sc.">B.Sc.</option>
							 <option value="B.Tech">B.Tech</option>
							 <option value="MBA">MBA</option>
						</select>
					</div>
					<div class="box-left">
						     Year:<select name="adm_clg_cls_std">
							<option value="default">Select One</option>
							 <option value="first">First</option>
							 <option value="second">Second</option>
							 <option value="three">Three</option>
							 <option value="four">Four</option>
							 <option value="fifth">Fifth</option>
						</select>
					</div>
					<div class="box-right">
					    Division Count:<input type="text" name="adm_clg_cls_div">
					</div>
					<div class="box-right">
						<button class="adm_clg_cls_sub"><span>Submit</span></button>
					</div>
				</form>
			</div>
		</div>
	</div>
</div>
 <?php
	include('../attach/footer_clg.php');
?>