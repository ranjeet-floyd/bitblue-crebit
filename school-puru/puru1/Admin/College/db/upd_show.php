<?php
include('../../db.php');
$set=$_POST['block'];
$id=mysqli_fetch_assoc(mysqli_query($con,"SELECT * from user_sch	
	where Gr_num='".$set['data']['Gr_num']."'
	")
);

?>
<br/><br/><br/><br/>	
<form class="adm_upd_std">
<div class="box-left">
	 Name <input  value="<?php echo $id['Name']?>" type="text" name="adm_upd_name">
</div>
<div class="box-right">
	Gr. No. <input  value="<?php echo $id['Gr_num']?>" type="text" name="adm_upd_grn">
</div>
<div class="box-left">
	 Medium:<select name="adm_upd_mdm">
		 <option value="English">English</option>
		 <option value="Marathi">Marathi</option>
	</select>
</div>
<div class="box-right" >
	 Standard: <select name="adm_upd_std">
							 <option value="first">First</option>
							 <option value="second">Second</option>
							 <option value="three">Three</option>
							 <option value="four">Four</option>
							 <option value="fifth">Fifth</option>
							 <option value="six">Six</option>
							 <option value="seven">Seven</option>
							 <option value="eight">Eight</option>
							 <option value="nine">Nine</option>
							 <option value="ten">Ten</option>
						</select>
</div>
<div class="box-left">
	 Division: <input  value="<?php echo $id['Section']?>" type="text" name="adm_upd_div">
</div>

<div class="box-right">
	<button class="adm_upd_sub"><span>Update</span></button>
</div>
</form>
<?php
exit;
?> 