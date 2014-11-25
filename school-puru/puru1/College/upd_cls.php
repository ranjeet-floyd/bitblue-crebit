<?php
	include('../attach/header_clg.php');
	?>
<div class="span-right">
	<div class="main-container">
		 <div class="post-header">
			<span> Select Class</span>
		 </div>
		 <div class="post-content">
			<div class="post-text">
				<form class="sch_updc_show" method="POST" action="">
				   <div class="box-left">
						 Medium:<select name="sch_updc_show_mdm">
							<option value="default">Select one</option>
							 <option value="English">English</option>
							 <option value="Marathi">Marathi</option>
						</select>
					</div>
					<div class="box-right">
						Course:<select  class="mando" name="sch_updc_show_cor">
							<option value="default">Select One</option>
							 <option value="B.A.">B.A.</option>
							 <option value="B.Com">B.Com</option>
							 <option value="B.Sc.">B.Sc.</option>
							 <option value="B.Tech">B.Tech</option>
							 <option value="MBA">MBA</option>
						</select>
					</div>
					<div class="box-left" onchange="div_count1()">
					     Year:<select name="sch_updc_show_std">
							<option value="default">Select one</option>
							 <option value="first">First</option>
							 <option value="second">Second</option>
							 <option value="three">Three</option>
							 <option value="four">Four</option>
						</select>
					</div>
					<div class="box-right div-update-show">
						 Division:	 
					</div>
					<div class="box-right">
							<button class="sch_updc_show_sub"><span>Show</span></button>
					</div>
				</form>
			</div>
		 </div>
	</div>
	<div class="updc_show_div">
	 </div>
	 	<div class="main-container">
		<div class="post-header">
			<span>Existing Classes</span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<ul class="table-view">
					<li class="table-view-header"style="width:65px;">
						Serial
					</li>
					<li class="table-view-header" style="width:65px;">
						Medium
					</li>
					<li class="table-view-header" style="width:65px;">
						Course
					</li>
					<li class="table-view-header" style="width:100px;">
						Year
					</li>
					<li class="table-view-header" style="width:100px;">
						Division Count
					</li>
					<li class="table-view-header" style="width:100px;">
						Strength
					</li>
				</ul>
				<?php

					$query=mysqli_query($con,"select * from clg_class");
					$i=0;
					while($result=mysqli_fetch_row($query)){
					$i++;
				?>	
				<ul class="table-view <?php echo 'view-'.$i ;?> ">
					<li  style="width:65px;">
						<?php echo $i; ?>
					</li >
					<li  style="width:65px;">
						<?php echo $result[0]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $result[1]; ?>
					</li>
					<li style="width:100px;">
						<?php echo $result[2]; ?>
					</li>
					<li style="width:100px;">
						<?php echo $result[3]; ?>
					</li>
					<li style="width:100px;">
						<?php
							$count=mysqli_num_rows(mysqli_query($con,"select * from user_clg where Medium='".$result[0]."' AND course='".$result[1]."'AND std='".$result[2]."'"));
							echo $count;
						?>
					</li>
					
				</ul>
				<?php 
				}
				
				?>
			</div>
		</div>
	</div>
</div>
	<?php
	include('../attach/footer_clg.php');
	
?>