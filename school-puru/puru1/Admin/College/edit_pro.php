<?php
	include('../attach/header_clg.php');
?>
<div class="span-right">	
	<div class="main-container">
		<div class="post-header">
			<span> Edit Account</span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<form class="header-name" action="" method="POST">
					 <div class="box-left">
						Name Of Institute <input  type="text" name="header-name">
					</div>
					<div class="box-right">
						<button class="header-name-sub"><span>Update</span></button>
					</div>
				</form>
			</div>
			<div class="text-line"><span > Upload Logo</span>
			</div>
			<form enctype="multipart/form-data" action="header-image.php" method="POST">
				<div class="box-left">
					<input type="hidden" name="MAX_FILE_SIZE" value="3000000" />
					<!-- Name of input element determines name in $_FILES array -->
					Upload logo: <input name="himg" type="file" />*Rename mage to logo.jpg/png
				</div>
				<br/>
				<div class="box-right">
					<button class="header-image-sub"><span>Update</span></button>
				</div>
			</form>
			
		</div>
	 </div>
	 <div class="sch_fee_div">
	 </div>
	 


	<?php
	include('../attach/footer_clg.php');
?>
