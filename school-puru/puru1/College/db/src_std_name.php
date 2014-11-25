
<?php
include('../../db.php');
parse_str($_POST['sch_src_name'], $set);
//print_r($set['adm_upd_show_grn']);
$query=mysqli_query($con,"SELECT Name,Gr_num from user_clg 	
	where Name LIKE '%".$set['sch_src_name']."%'
	");
	
$i=0;
if(mysqli_num_rows($query)==0){
	?>
	<p>No Student of this Name</p>
	<?php
	exit;
}
?>
<ul class="table-view">
	<li class="table-view-header" style="width:150px;">
		Name
	</li>
	<li class="table-view-header" style="width:150px;">
		Gr Number
	</li>
	<li class="table-view-header" style="width:150px;">
		Update
	</li>
</ul>

<?php
$i=0;
while($id=mysqli_fetch_row($query)){
?>
<ul class="table-view">
	<li style="width:150px;"><?php echo $id[0];?></li>
	<li class='<?php echo("name-search-2-".$i);?>' style="width:150px;"><?php echo $id[1];?></li>
	<li style="width:150px;"><button class="name-search-2-sub" id='<?php echo("name-search-2-".$i);?>'><span>Show</span> </button></li>
</ul>
<?php 
$i++;
}
?>