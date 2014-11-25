<?php
	include('../attach/header_sch.php');
	//echo($fcount);
	
?>
<div class="span-right">
	<div class="main-container">
		<div class="post-header">
			<span>View Class Fees</span>
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
						Fee type
					</li>
					<li class="table-view-header" style="width:65px;">
						Fee
					</li>
					<li class="table-view-header" style="width:65px;">
						Late fee 
					</li >
					<li class="table-view-header" style="width:65px;">
						Frequency
					</li >
					<li class="table-view-header" style="width:65px;">
						Fee count
					</li >
				</ul>
				<?php


					$query=mysqli_query($con,"select * from sch_cls_fee");
					$i=0;
					if(mysqli_num_rows($query)!=0){
					while($result=mysqli_fetch_row($query)){
					$i++;
					$count=0;
					$fcount=mysqli_query($con,"select user_sch.Medium,user_sch.Std,sch_tran.fee_type 
						from user_sch 
						inner join sch_tran 
						on user_sch.Gr_num=sch_tran.Gr_num
						
					");
					while($set=mysqli_fetch_row($fcount)){
						if($result[0]==$set[0] && $result[1]==$set[1] && $result[2]==$set[2])
							$count++;
					}
					
				?>	
				<ul class="table-view ">
					<li style="width:65px;">
						<?php echo $i; ?>
					</li >
					<li style="width:65px;">
						<?php echo $result[0]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $result[1]; ?>
					</li>
					<li style="width:100px;">
						<?php echo $result[2]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $result[3]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $result[4]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $result[5]; ?>
					</li>
					<li style="width:65px;">
						<?php 
						echo $count;
						?>
					</li>
				</ul>
				<?php
					}
					}
				?>
				<span style="display:none" class="table-view-id">
				<?php echo $i++?>
				</span>
				<div class="table-view-result">
				
				</div>
			</div>
		</div>
	</div>
	<div class="main-container">
		<div class="post-header">
			<span>Set Fees</span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<form class="adm_set"  method="post">
					<div class="box-left">
						 Medium:<select name="adm_set_mdm">
						 <option value="default">Select One</option>
							<option value="English">English</option>
							 <option value="Hindi">Hindi</option>
							 <option value="Marathi">Marathi</option>
						</select>
					</div>
					<div class="box-right">
						 Standard:<select name="adm_set_std">
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
						 Fee type: <select name="adm_set_fee_type">
							 <option value="default">Select One</option>
							 <option value="Admission Fee">Admission</option>
								 <option value="Monthly Fee">Monthly</option>
								 <option value="Mid term 1 fee">Mid term 1</option>
								 <option value="Mid term 2 fee">Mid term 2</option>
								 <option value="End Term Fee">End Term</option>
								 <option value="Sports">Sports</option>
								 <option value="Computer Lab">Computer Lab</option>
								 <option value="Practical">Practical</option>
								 <option value="Insurance">Insurance</option>
								 <option value="Summer Camp">Summer Camp</option>
							 </select>
					</div>
					<div class="box-right">
					     Fees: <input  type="text" name="adm_set_fee">
					</div>
					<div class="box-left">
						 Late fee:<input  type="text" name="adm_set_lfee">
					</div>
					<div class="box-right">
						 Frequency:
							<br/>
						<label class="checkbox-text"><input  type="radio" name="adm_set_one" value ="Per Year">Per Year</label><br/>
						<label class="checkbox-text"><input  type="radio" name="adm_set_one" value="Per Month">Per Month</label>
							<button class="adm_set_sub_button"><span>Add</span></button>
					</div>
	
				</form>
			
			</div>
		</div>
	</div>
		
</div>
 <?php
	include('../attach/footer_sch.php');
?>