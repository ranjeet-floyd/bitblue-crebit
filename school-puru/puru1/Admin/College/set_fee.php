	<?php
	include('../attach/header_clg.php');
	
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
						Course
					</li>
					<li class="table-view-header" style="width:65px;">
						Year
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


					$query=mysqli_query($con,"select * from clg_cls_fee order by medium,course,std");
					$i=0;
					if(mysqli_num_rows($query)!=0){
					while($result=mysqli_fetch_row($query)){
					$i++;
					$count=0;
					$fcount=mysqli_query($con,"select user_clg.Medium,user_clg.Std,clg_tran.fee_type 
						from user_clg 
						inner join clg_tran 
						on user_clg.Gr_num=clg_tran.Gr_num
						
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
					<li style="width:65px;">
						<?php echo $result[2]; ?>
					</li>
					<li style="width:100px;">
						<?php echo $result[3]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $result[4]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $result[5]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $result[6]; ?>
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
							 <option value="Marathi">Marathi</option>
						</select>
					</div>
					<div class="box-right">
						Course:<select name="adm_set_cor">
							 <option value="default">Select One</option>
							  <option value="B.A.">B.A.</option>
							 <option value="B.Com">B.Com</option>
							 <option value="B.Sc.">B.Sc.</option>
							 <option value="B.Tech">B.Tech</option>
							 <option value="MBA">MBA</option>
						</select>
					</div>
					<div class="box-left">
						 Year:<select name="adm_set_std">
							<option value="default">Select One</option>
							 <option value="first">First</option>
							 <option value="second">Second</option>
							 <option value="three">Three</option>
							 <option value="four">Four</option>
						</select>
					</div>
					<div class="box-right">
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
					<div class="box-left">
					     Fees: <input  type="text" name="adm_set_fee">
					</div>
					<div class="box-right">
						 Late fee:<input  type="text" name="adm_set_lfee">
					</div>
					<div class="box-left">
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
	include('../attach/footer_clg.php');
?>