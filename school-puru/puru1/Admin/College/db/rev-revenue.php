	<div class="main-container">
		<div class="post-header">
			<span>View Revenue by Class Fees</span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<ul class="table-view">
					<li class="table-view-header"style="width:65px;">
						Medium
					</li>
					<li class="table-view-header"style="width:65px;">
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
					</li>
					<li class="table-view-header" style="width:65px;">
						Payee count
					</li>
					<li class="table-view-header" style="width:100px;">
						Frequency
					</li >
				</ul>
				<?php

					include('../../db.php');
					$query=mysqli_query($con,"select * from clg_cls_fee");
				
					if(mysqli_num_rows($query)!=0){
					while($result=mysqli_fetch_row($query)){
					$count=mysqli_num_rows(mysqli_query($con,"select * from user_clg where medium='".$result[0]."' AND course='".$result[1]."' And std='".$result[2]."'"));
					
					
					
				?>	
				<ul class="table-view ">
					<li style="width:65px;">
						<?php echo $result[0]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $result[1]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $result[2]; ?>
					</li>
					<li  style="width:100px;">
						<?php echo $result[3]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $result[4]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $result[5]; ?>
					</li>
					<li style="width:65px;">
						<?php echo $count; ?>
					</li>	
					<li style="width:100px;">
						<?php echo $result[6]; ?>
					</li>
						</ul>
				<?php
					}
					}
				?>
				
					*payee count: Number of students who has the same class as fee
			</div>
		</div>
	