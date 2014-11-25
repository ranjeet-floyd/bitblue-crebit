 <?php
	include('../attach/header_clg.php');
?>
<div class="span-right">
	<div class="main-container">
		<div class="post-header">
			<span>Search Student</span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<form class="adm_sch_src" method="post">
					<div class="box-left">
						Gr. No. <input  type="text" name="adm_sch_src_grn">
					</div>
					<div class="box-right">
						<button class="adm_sch_src_sub"><span>Search</span></button>
					</div>
				</form>
			</div>
		</div>
		<div class="text-line"><span >or Search By Name</span></div>
		<div class="post-content">
			<div class="post-text">
				<form class="adm_sch_src_show_name" method="post" action="">
					<div class="box-left">
						Name. <input  type="text" name="adm_sch_src_name" >
					</div>
					<div class="box-right">
						<button class="adm_sch_src_name_sub"><span>Search</span></button>
					</div>
				</form>
				<div class="post-text dyn_src_details_name">
			
				</div>
			</div>
		</div>
	</div>
	<div class="search-div">
	
	</div>
</div>
 <?php
	include('../attach/footer_clg.php');
?>