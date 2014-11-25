<?php
	include('../attach/header_sch.php');
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
							 <option value="Hindi">Hindi</option>
							 <option value="Marathi">Marathi</option>
						</select>
					</div>
					<div class="box-right">
					     Standard:<select name="sch_updc_show_std" onchange="div_count1()">
							<option value="default">Select one</option>
							     <option value="Mr.dextro">Mr.dextro</option>
							 <option value="nursery">Nursery</option>
                             <option value="junior.kg">jr.kg</option>
                             <option value="senior.kg">sr.kg</option>
							 <option value="first">First</option>
							 <option value="second">Second</option>
							 <option value="third">Third</option>
							 <option value="fourth">Fourth</option>
							 <option value="fifth">Fifth</option>
							 <option value="sixth">Sixth</option>
							 <option value="seventh">Seventh</option>
							 <option value="eighth">Eighth</option>
							 <option value="ninth">Ninth</option>
							 <option value="tenth">Tenth</option>
							 
						</select>
					</div>
					<div class="box-left div-update-show">
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
						Standard
					</li>
					<li class="table-view-header" style="width:100px;">
						No of Div
					</li>
					<li class="table-view-header" style="width:100px;">
						Strength
					</li>
				</ul>
				<?php

					$query=mysqli_query($con,"select * from sch_class");
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
						<?php
							$count=mysqli_num_rows(mysqli_query($con,"select * from user_sch where Medium='".$result[0]."' AND std='".$result[1]."'"));
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
	include('../attach/footer_sch.php');
	
?>