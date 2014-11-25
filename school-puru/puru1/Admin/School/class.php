 <?php
	include('../attach/header_sch.php');	
	$query=mysqli_query($con,"select * from sch_class ");
	$i=0;
	if(mysqli_num_rows($query)){
	?>
	<div class="span-right">
		<div class="main-container">
			<div class="post-header">
				<span>View Existing Class</span>
			</div>
			<div class="post-content">
				<div class="post-text">
					<ul class="table-view">
						<li class="table-view-header" style="width:100px">
							Medium
						</li>
						<li class="table-view-header" style="width:100px;">
							Standard
						</li>
						<li class="table-view-header" style="width:100px;">
							Division
						</li>
						<li class="table-view-header" style="width:100px;">
							Strength
						</li>
						
							
					</ul>	
					<?php	
						while($result=mysqli_fetch_row($query)){
							$i++;
							//	print_R($result);
					?>
					<ul class="table-view  ">
					
						<li style="width:100px" >
							<?php echo $result[0]; ?>
						</li>
						<li  style="width:100px;">
							<?php echo $result[1]; ?>
						</li>
						<li  style="width:100px;">
							<?php echo $result[2]; ?>
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
			</div>
		</div>
	</div>
	<?php
	}
	?>
		<div class="main-container">
		<div class="post-header">
			<span>Create Class</span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<form class="adm_cls"  method="post">
					<div class="box-left">
						 Medium:<select name="adm_cls_mdm">
							 <option value="default">Select One</option>
							 <option value="English">English</option>
							 <option value="Hindi">Hindi</option>
							 <option value="Marathi">Marathi</option>
						</select>
					</div>
					<div class="box-right">
						 Standard:<select name="adm_cls_std">
							 <option value="default">Select One</option>
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
					<div class="box-left">
					     Division Count: <input  type="text" name="adm_cls_div">
					</div>
					<div class="box-right">
						<button class="adm_cls_sub"><span>Submit</span></button>
					</div>
				</form>
			</div>
		</div>
	</div>
</div>
 <?php
	include('../attach/footer_sch.php');
?>