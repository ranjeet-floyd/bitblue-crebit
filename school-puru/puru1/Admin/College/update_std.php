 <?php
	include('../attach/header_clg.php');
?>
<div class="span-right">
	<div class="main-container">
		<div class="post-header">
			<span>Update Student Info.</span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<form class="adm_upd_show" method="post" action="">
					<div class="box-left">
						Gr. No. <input  type="text" name="adm_upd_show_grn" >
					</div>
					<div class="box-right">
						<button class="adm_upd_show_sub"><span>Show</span></button>
					</div>
				</form>
				<div class="dyn_upd_details">
					
				</div>
			</div>
		</div>
		<div class="text-line"><span >or Search By Name</span></div>
		<div class="post-content">
			<div class="post-text">
				<form class="adm_upd_show_name" method="post" action="">
					<div class="box-left">
						Name. <input  type="text" name="adm_upd_show_name" >
					</div>
					<div class="box-right">
						<button class="adm_upd_show_name_sub"><span>Search</span></button>
					</div>
				</form>
				<div class="post-text dyn_upd_details_name">
			
				</div>
			</div>
		</div>
	</div>
</div>
 <?php
	include('../attach/footer_clg.php');
?>
